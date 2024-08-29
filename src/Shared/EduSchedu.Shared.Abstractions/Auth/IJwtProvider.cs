﻿namespace EduSchedu.Shared.Abstractions.Auth;

public interface IJwtProvider
{
    public string GenerateToken(string userId, string email);
}