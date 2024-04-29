using CustomersAPI.Models;
using CustomersAPI.Models.Requests;
using Riok.Mapperly.Abstractions;

namespace CustomersAPI.Mapper
{
    [Mapper]
    public partial class CustomerMapper 
    {
        public partial Customer CustomerCreateRequestToCustomer(CustomerCreateRequest customerCreateRequest);

        public partial Customer CustomerUpdateRequestToCustomer(CustomerUpdateRequest customerUpdateRequest);
    }
}
