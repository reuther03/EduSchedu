﻿using EduSchedu.Modules.Schools.Application.Abstractions.Database;
using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Shared.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EduSchedu.Modules.Schools.Application;

[Authorize]
public class ClassChatHub : Hub
{
    private readonly IUserService _userService;
    private readonly ISchoolsDbContext _context;

    private const string ReceiveMessage = "ReceiveMessage";

    public ClassChatHub(IUserService userService, ISchoolsDbContext context)
    {
        _userService = userService;
        _context = context;
    }

    public override async Task OnConnectedAsync()
    {
        var user = await _context.SchoolUsers.FindAsync(_userService.UserId);
        await Clients.All.SendAsync(ReceiveMessage, "System", $"{user.FullName} connected.");
    }

    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync(ReceiveMessage, user, message);
    }

    public async Task JoinClass(string schoolId, string classId)
    {
        var user = await _context.SchoolUsers.FindAsync(_userService.UserId);
        if (user == null)
            throw new InvalidOperationException("User not found.");

        var school = await _context.Schools.FindAsync(SchoolId.From(schoolId));
        if (school == null)
            throw new InvalidOperationException("School not found.");

        if (!school.TeacherIds.Contains(user.Id))
            throw new InvalidOperationException("User is not a teacher in this school.");

        var @class = await _context.Classes.FindAsync(ClassId.From(classId));
        if (@class == null)
            throw new InvalidOperationException("Class not found.");

        if (@class.Lessons.Any(x => x.AssignedTeacher == user.Id))
            throw new InvalidOperationException("User is not a teacher in this class.");

        // t
        await Groups.AddToGroupAsync(Context.ConnectionId, @class.Id);
        await Clients.Group(classId).SendAsync(ReceiveMessage, "System", $"joined the class {classId}.");
    }

    // public async Task JoinClass(string user, string classId)
    // {
    //     await Groups.AddToGroupAsync(Context.ConnectionId, classId);
    //     await Clients.Group(classId).SendAsync("ReceiveMessage", "System", $"{user} joined the class {classId}");
    // }
}