using CustomersAPI;
using CustomersAPI.Repositories;
using FluentResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

public class CustomerRepository<TEntity> : ICustomerRepository<TEntity> where TEntity : class
{
    private readonly CustomerDbContext _context;

    public CustomerRepository(CustomerDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Result<TEntity>> GetByIdAsync(Guid id)
    {
        try
        {
            TEntity entity = await _context.FindAsync<TEntity>(id);

            if (entity == null)
            {
                return Result.Fail<TEntity>("Entity not found.");
            }

            return Result.Ok(entity);
        }
        catch (Exception ex)
        {
            // Handle any exceptions and return a failure result
            return Result.Fail<TEntity>(ex.Message);
        }
    }

    public async Task<Result<IEnumerable<TEntity>>> GetAllAsync()
    {
        try
        {
            var entities = await _context.Set<TEntity>().ToListAsync();
            IEnumerable<TEntity> entityEnumerable = entities;
            return Result.Ok(entityEnumerable);
        }
        catch (Exception ex)
        {
            // Handle exceptions
            return Result.Fail<IEnumerable<TEntity>>(ex.Message);
        }
    }

    public async Task<Result<TEntity>> AddAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _context.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Result<TEntity>> UpdateAsync(Guid id, TEntity updatedEntity)
    {
        try
        {
            var existingEntity = await _context.Set<TEntity>().FindAsync(id);
            if (existingEntity == null)
            {
                // If no existing entity found, return failure result
                var error = new Error($"Entity with ID {id} not found.");
                return Result.Fail<TEntity>(new CustomError(HttpStatusCode.BadRequest,$"Entity with ID {id} not found."));
            }

            // Update the existing entity with the values from the updated entity
            _context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);

            // Set the state of the updated entity to Modified
            _context.Entry(existingEntity).State = EntityState.Modified;

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Result.Ok(existingEntity);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return Result.Fail<TEntity>(new CustomError(HttpStatusCode.InternalServerError,"Concurrency conflict occurred.").CausedBy(ex.Message));
        }
        catch (DbUpdateException ex)
        {
            return Result.Fail<TEntity>(new CustomError(HttpStatusCode.InternalServerError, "Database update error occurred.").CausedBy(ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Fail<TEntity>(new CustomError(HttpStatusCode.InternalServerError, "Unexpected exception").CausedBy(ex.Message));
        }
    }



    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        try
        {
            var entityToDelete = await _context.FindAsync<TEntity>(id);

            if (entityToDelete != null)
            {
                _context.Remove(entityToDelete);
                await _context.SaveChangesAsync();
                return Result.Ok(true);
            }
            else
            {
                return Result.Fail<bool>("Entity not found for deletion.");
            }
        }
        catch (DbUpdateException ex)
        {
            return Result.Fail<bool>(new CustomError(HttpStatusCode.InternalServerError, "Database update error occurred while deleting the entity.").CausedBy(ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Fail<bool>(new CustomError(HttpStatusCode.InternalServerError, "Unexpected exception").CausedBy(ex.Message));
        }
    }

    public async Task<Result<TEntity>> PatchAsync(Guid id, JsonPatchDocument<TEntity> customerModel)
    {
        TEntity entity = await _context.FindAsync<TEntity>(id);
        if (entity != null)
        {
            // Apply the patch
            customerModel.ApplyTo(entity);

            // Validate the patched entity
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults, validateAllProperties: true);

            if (!isValid)
            {
                // If validation fails, return validation errors
                var errorMessages = validationResults.Select(vr => vr.ErrorMessage);
                return Result.Fail(new CustomError(HttpStatusCode.BadRequest,string.Join("\n", errorMessages)));
            }

            // If validation passes, save changes
            await _context.SaveChangesAsync();
            return Result.Ok(entity);
        }
        else
        {
            return Result.Fail(new CustomError(HttpStatusCode.BadRequest,"Unable to find Entity"));
        }
    }

}
