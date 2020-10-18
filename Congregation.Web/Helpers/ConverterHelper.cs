using Congregation.Common.Request;
using Congregation.Common.Responses;
using Congregation.Web.Data;
using Congregation.Web.Data.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Congregation.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        //private readonly DataContext _context;

        //public ConverterHelper(DataContext context)
        //{
        //    _context = context;
        //}

        //public async Task<MeetingResponse> ToMeetingResponse(Meeting meeting)
        //{
        //    return new MeetingResponse
        //    {
        //        Date = meeting.Date,
        //        Assistances = _context.Assistances.FindAsync(meeting.Assistances.Id), //(ICollection<AssistanceResponse>)await _context.Meetings.FindAsync(meeting.Id),
        //        Church = await _context.Churches.FindAsync(meeting.Church),
        //        ChurchId = meeting.Church.Id,
                
        //    };
        //}
    }
}
