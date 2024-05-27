using System.Text.Json;

namespace Application.DTO
{
    public class HolidayGatewayDTO
    {
        public long colaboratorId {get; set;}

        public HolidayGatewayDTO(){}
        
        public HolidayGatewayDTO(long colabId)
        {
            colaboratorId = colabId;
        }

         public static string Serialize(HolidayDTO holidayDTO)
        {
            var jsonMessage = JsonSerializer.Serialize(holidayDTO);
            return jsonMessage;
        }

        public static HolidayDTO Deserialize(string jsonMessage)
        {
            var holidayAmqpDTO = JsonSerializer.Deserialize<HolidayDTO>(jsonMessage);
            return holidayAmqpDTO!;
        }
    }
}