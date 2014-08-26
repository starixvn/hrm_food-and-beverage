// --------------------------------------------------------------------------
// <copyright file="LoginModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.8.26">Created</change>
// </history>

namespace NexFB.Models.Authority
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Login Model.
    /// </summary>
    public class LoginModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
