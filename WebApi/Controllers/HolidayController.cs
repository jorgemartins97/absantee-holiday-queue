using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Application.DTO;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayController : ControllerBase
    {   
        private readonly HolidayService _holidayService;
        private List<string> _errorMessages = new List<string>();

        public HolidayController(HolidayService holidayService)
        {
            _holidayService = holidayService;
        }

        // GET: api/Holiday
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HolidayDTO>>> GetHolidays()
        {
            IEnumerable<HolidayDTO> holidaysDTO = await _holidayService.GetAll();
            return Ok(holidaysDTO);
        }

        // GET: api/Holiday/1
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<HolidayDTO>>> GetHolidaysById(long id)
        {
             IEnumerable<HolidayDTO> holidaysDTO = await _holidayService.GetHolidayById(id);
            if (holidaysDTO == null)
            {
                return NotFound();
            }
            return Ok(holidaysDTO);
        }

        // GET: api/Holiday/periods/1
        [HttpGet("periods/{colabId}")]
        public async Task<ActionResult<List<HolidayPeriodDTO>>> GetHolidayPeriodsOnHolidayById(long colabId,[FromQuery] DateOnly startDate,[FromQuery] DateOnly endDate)
        {
            IEnumerable<HolidayPeriodDTO> holidayPeriodDTOs = await _holidayService.GetHolidayPeriodsOnHolidayById(colabId,startDate,endDate,_errorMessages);
            if (holidayPeriodDTOs == null)
            {
                return NotFound();
            }
            return Ok(holidayPeriodDTOs);
        }

        // GET: api/Holiday/1/colabsComFeriasSuperioresAXDias
        [HttpGet("{xDias}/colabsComFeriasSuperioresAXDias")]
        public async Task<ActionResult<List<long>>> GetColabsComFeriasSuperioresAXDias(long xDias)
        {
            List<long> colabsComFeriasSuperioresAXDias = await _holidayService.GetColabsComFeriasSuperioresAXDias(xDias,_errorMessages);
            if (colabsComFeriasSuperioresAXDias == null)
            {
                return NotFound();
            }
            return Ok(colabsComFeriasSuperioresAXDias);
            return Ok();
        }

    }
}
