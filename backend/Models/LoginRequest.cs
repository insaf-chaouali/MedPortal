using System;
using System.Collections.Generic;

namespace projet_1.Models
{

public class LoginRequest
{
    public required string Login { get; set; }
    public required string Password { get; set; }

}
}