using Microsoft.AspNetCore.Http;
using System;

namespace Raspisanie.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}