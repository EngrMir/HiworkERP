using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.Utils.Infrastructure
{
    public class JobScheduler
    {
        public static IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

        public static void Start()
        {

            scheduler.Start();

            IJobDetail SendEmailjob = JobBuilder.Create<EmailJob>().Build();


            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInMinutes(5)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))

                  )
                .Build();


            //This will create a trigger, running every 45 minutes, between 8am and 11am.

            ITrigger emailtrigger = TriggerBuilder.Create()
                 //.WithIdentity("trigger1", "group1")
                .WithDailyTimeIntervalSchedule(
                   x => x.StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(8, 0))  
                 .EndingDailyAt(TimeOfDay.HourAndMinuteOfDay(11, 0))
                 .WithIntervalInMinutes(45))
               .Build();

         //   scheduler.ScheduleJob(SendEmailjob, emailtrigger);
            scheduler.ScheduleJob(SendEmailjob, emailtrigger);

        }
    }


    public class EmailJob : IJob
    {
       
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                MailMessage emailMessage = new MailMessage();
                emailMessage.From = new MailAddress("mojahid.cse08@gmail.com", "Mojahid");
                emailMessage.To.Add(new MailAddress("akd.bracu@gmail.com", "Ashis"));
                emailMessage.Subject = "SUBJECT";
                emailMessage.Body = "Hello Ashis... How are you?";
                emailMessage.Priority = MailPriority.Normal;
                SmtpClient MailClient = new SmtpClient("smtp.gmail.com", 587);
                MailClient.EnableSsl = true;
                MailClient.Credentials = new System.Net.NetworkCredential("mojahid.cse08@gmail.com", "password");
                MailClient.Send(emailMessage);
            }
            catch(Exception ex)
            {

            }

        }
    }
}

