using Microsoft.AspNetCore.Mvc;
using RouletteAPI.Interfaces;

namespace RouletteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpinResultController : ControllerBase
    {

        private readonly ISpinResultRepository _spinResultRepository;
        private readonly ISpinResultHelper _spinResultHelper;
        public SpinResultController(ISpinResultRepository spinResultRepository, ISpinResultHelper spinResultHelper)
        {
            _spinResultRepository = spinResultRepository;
            _spinResultHelper = spinResultHelper;
        }

        [Route("SpinTheWheel")]
        [HttpPost]
        public async Task<JsonResult> SpinTheWheel()
        {
            int result = _spinResultHelper.SpinTheWheel();
            return await AddSpinResult(result);
        }

        [Route("AddSpinResult")]
        [HttpPost]
        public async Task<JsonResult> AddSpinResult(int result)
        {
            var spinIdNumber = await  _spinResultRepository.AddSpinResultAsync(result);
            return new JsonResult(spinIdNumber);
        }


        [Route("GetAllSpinResults")]
        [HttpGet]
        public async Task<JsonResult> GetAllSpinResults()
        {
            var table = await _spinResultRepository.GetAllSpinResultsAsync();
            return new JsonResult(table);
        }

        [Route("GetSpinResult")]
        [HttpGet]
        public async Task<JsonResult> GetSpinResult(int spinResultId)
        {
            var table = await _spinResultRepository.GetSpinResultAsync(spinResultId);
            return new JsonResult(table);
        }

        [Route("GetLatestSpinResult")]
        [HttpGet]
        public async Task<JsonResult> GetLatestSpinResult()
        {
            var table = await _spinResultRepository.GetLatestSpinResultAsync();
            return new JsonResult(table);
        }

    }
}
