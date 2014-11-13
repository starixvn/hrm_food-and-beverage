// --------------------------------------------------------------------------
// <copyright file="ChangePasswordModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.11.12">Created</change>
// </history>

namespace NexFB.Models.Authority
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Change Password Model.
    /// </summary>
    public class ChangePasswordModel
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}
