﻿using Congregation.Common.Request;
using Congregation.Common.Responses;
using Congregation.Web.Data;
using Congregation.Web.Data.Entities;
using Congregation.Web.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities.Zlib;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Congregation.Web.Controllers.API
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]//requiere token
    [Route("api/[controller]")]
    public class MeetingsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public MeetingsController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] MeetingRequest request)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }

        //    string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;//traigo el email del usuario logeado
        //    User user = await _userHelper.GetUserAsync(email);
        //    if (user == null)
        //    {
        //        return NotFound("User not found");
        //    }

        //    Church church = await _context.Churches
        //        .Include(c => c.Users)
        //        .FirstOrDefaultAsync(c => c.Id == user.Church.Id);
        //    if (church == null)
        //    {
        //        return NotFound("Church no found");
        //    }

        //    Meeting meeting = await _context.Meetings
        //        .Include(m => m.Church)
        //        .Include(m => m.Assistances)
        //        .ThenInclude(a => a.User)
        //        .ThenInclude(u => u.Profession)
        //        .FirstOrDefaultAsync(m => m.Date.Year == request.Date.Year &&
        //                                    m.Date.Month == request.Date.Month &&
        //                                    m.Date.Day == request.Date.Day &&
        //                                    m.Church.Id == user.Church.Id);

        //    if (meeting == null)
        //    {
        //        List<User> users = _context.Users.Where(u => u.Church == church).ToList();

        //        Meeting meetingNew = new Meeting
        //        {
        //            Church = church,
        //            Date = request.Date,
        //        };

        //        foreach(var item in users)
        //        {
        //            User userList = _context.Users.FirstOrDefault(u => u.Id == item.Id);
        //            Assistance assistance = new Assistance
        //            {
        //                Meeting = meetingNew,
        //                User = userList,
        //                IsPresent = false
        //            };

        //            _context.Assistances.Add(assistance);

        //        }
        //        await _context.SaveChangesAsync();
        //        return Ok(meetingNew);

        //    }
        //    else
        //    {
        //        return Ok(meeting);
        //    }
        //}



        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MeetingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;//traigo el email del usuario logeado
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("User not found");
            }

            Church church = await _context.Churches
                .Include(c => c.Users)
                .FirstOrDefaultAsync(c => c.Id == user.Church.Id);
            if (church == null)
            {
                return NotFound("Church no found");
            }

            Meeting meeting = await _context.Meetings
                .Include(m => m.Church)
                .Include(m => m.Assistances)
                .ThenInclude(a => a.User)
                .ThenInclude(u => u.Profession)
                .FirstOrDefaultAsync(m => m.Date.Year == request.Date.Year &&
                                            m.Date.Month == request.Date.Month &&
                                            m.Date.Day == request.Date.Day &&
                                            m.Church.Id == user.Church.Id);


            bool isNew = false;
            if(meeting == null)
            {
                isNew = true;
                meeting = new Meeting
                {
                    Assistances = new List<Assistance>(),
                    Church = church,
                    Date = request.Date
                };
            }
            
            foreach(User membersChurch in church.Users)
            {
                Assistance assistance = meeting.Assistances.FirstOrDefault(m => m.User.Id == membersChurch.Id);
                if(assistance == null)
                {
                    meeting.Assistances.Add(new Assistance
                    {
                        User = membersChurch
                    });
                }
            }

            if (isNew)
            {
                _context.Meetings.Add(meeting);
            }
            else
            {
                _context.Meetings.Update(meeting);
            }

            await _context.SaveChangesAsync();
            //return Ok(_converterHelper.ToMeetingResponse(meeting));
            return Ok(meeting);

        }

        [HttpPut]
        public async Task<IActionResult> PutMeeting(MeetingResponse meeting)
        {
            Meeting meetingUpdate = await _context.Meetings
                .Include(m => m.Assistances)
                .FirstOrDefaultAsync(m => m.Id == meeting.Id);
            if (meetingUpdate == null)
            {
                return BadRequest("Meeting not found");
            }

            foreach (AssistanceResponse assistance in meeting.Assistances)
            {
                Assistance assistanceUpdate = meetingUpdate.Assistances.FirstOrDefault(a => a.Id == assistance.Id);
                if (assistanceUpdate == null)
                {
                    meetingUpdate.Assistances.Add(new Assistance
                    {
                        IsPresent = assistance.IsPresent,
                        User = await _userHelper.GetUserAsync(assistance.User.Id)
                    });
                }
                else
                {
                    assistanceUpdate.IsPresent = assistance.IsPresent;
                }
            }

            _context.Meetings.Update(meetingUpdate);
            await _context.SaveChangesAsync();
            return Ok(meetingUpdate);

        }

        [HttpGet]
        public IActionResult GetMeetings()
        {
            return Ok(_context.Meetings
                .Include(c => c.Church)
                .Include(a => a.Assistances)
                .ThenInclude(u => u.User).ToList());


        }
    }
}