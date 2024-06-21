using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace StockMarket.Api.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected readonly IMapper _mapper;

        protected BaseApiController(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
