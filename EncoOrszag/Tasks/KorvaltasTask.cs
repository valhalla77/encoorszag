using EncoOrszag.Helpers;
using EncoOrszag.Models.DataAccess.Entities;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EncoOrszag.Tasks
{
   public class KorvaltasTask : IJob
   {

      public void Execute(IJobExecutionContext context)
      {
            using (var korvaltasHelper = new KorvaltasHelper())
            {
                korvaltasHelper.Korvaltas();
            }

      }


   }
}