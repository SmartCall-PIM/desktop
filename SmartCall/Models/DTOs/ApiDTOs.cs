using System;
using System.Collections.Generic;

namespace SmartCall.Models.DTOs
{
    // ========== Auth DTOs ==========
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public UserInfo? User { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }
    }

    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }

    public class UserInfo
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Role { get; set; } = "Usuário";
    }

    // ========== User Management DTOs ==========
    public class UserListResponse
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateUserRequest
    {
        public string Email { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? Role { get; set; }
    }

    public class CreateUserRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = "Usuário";
    }

    public class ApiResponse
    {
        public string? Message { get; set; }
        public bool RequiresRelogin { get; set; }
        public string? UpdatedUserId { get; set; }
    }

    public class ErrorResponse
    {
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }
    }
}
