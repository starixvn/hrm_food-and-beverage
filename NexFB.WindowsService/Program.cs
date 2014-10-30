// --------------------------------------------------------------------------
// <copyright file="Program.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.4">Created</change>
// </history>

namespace NexFB.WindowsService
{
    using System;
    using System.ServiceProcess;
    using Fanex.Data.Configuration;

    /// <summary>
    /// Program Class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            DbSettingProviderManager.Current.Start();
            log4net.Config.XmlConfigurator.Configure();
            ServiceBase[] servicesToRun;
            servicesToRun = new ServiceBase[] 
            { 
                new AutoService() 
            };
            ServiceBase.Run(servicesToRun);
        }

        ////public static void Main()
        ////{
        ////    DbSettingProviderManager.Current.Start();
        ////    var service = new CommonService();

        ////    service.UpdateTeamForNexcel();
        ////    service.UpdateUserForNexcel();
        ////    service.UpdateShiftForNexcel();
        ////    service.UpdateEmployeeCalendarForNexcel();
        ////    service.UpdatePhotoForNexcel();

        ////    service.UpdateTeamForHM();
        ////    service.UpdateUserForHM();
        ////    service.UpdateShiftForHM();
        ////    service.UpdateEmployeeCalendarForHM();
        ////    service.UpdatePhotoForHM();

        ////    service.UpdateTeamForSms();
        ////    service.UpdateUserForSms();
        ////    service.UpdateShiftForSms();
        ////    service.UpdateEmployeeCalendarForSms();
        ////    service.UpdatePhotoForSms();

        ////    Console.WriteLine("OK");
        ////    Console.ReadLine();
        ////}
    }
}
