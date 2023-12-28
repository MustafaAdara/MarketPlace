using AutoMapper;
using Marketplace.Dtos;
using Marketplace.Interfaces;
using Marketplace.Models;
using Marketplace.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarketController : Controller
    {
        private readonly IMarketRepository _marketRepository;
        private readonly IMapper _mapper;

        public MarketController(IMarketRepository marketRepository, IMapper mapper)
        {
            _marketRepository = marketRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Market>))]
        public IActionResult GetMarkets()
        {
            var markets =_mapper.Map<List<MarketDto>>(_marketRepository.GetMarkets());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(markets);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateMarket([FromBody] MarketDto createMarket)
        {
            if (createMarket == null) return BadRequest(ModelState);

            var isExisting = _marketRepository.GetMarkets().Where(c => c.Name.Trim() == createMarket.Name.TrimEnd()).FirstOrDefault();

            if (isExisting != null)
            {
                ModelState.AddModelError("", "Market Already Exists");
                return StatusCode(422, ModelState);
            }

            var marketMap = _mapper.Map<Market>(createMarket);

            if (!_marketRepository.CreateMarket(marketMap))
            {
                ModelState.AddModelError("", "Something Went wrong while creating market...");
                return StatusCode(500, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(marketMap);
        }

        [HttpPut("{marketId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public ActionResult UpdateMarket(int marketId, [FromBody]MarketDto updateMarket)
        {
            if (updateMarket == null) return BadRequest(ModelState);
            if (marketId == 0) return BadRequest(ModelState);
            if (marketId != updateMarket.Id) return BadRequest(ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);


            if (!_marketRepository.MarketExist(marketId))
                return NotFound();
            var marketMap = _mapper.Map<Market>(updateMarket);

            if (!_marketRepository.UpdateMarket(marketMap))
            {
                ModelState.AddModelError("", "somthing went wrong when updating...");
                return StatusCode(500, ModelState);
            }

            return Ok(marketMap);
        }

        [HttpDelete("{marketId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteMarket(int marketId)
        {
            if (!_marketRepository.MarketExist(marketId)) 
                return NotFound();

            var marketToDelete = _marketRepository.GetMarket(marketId);

            if(!_marketRepository.DeleteMarket(marketToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting...");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
