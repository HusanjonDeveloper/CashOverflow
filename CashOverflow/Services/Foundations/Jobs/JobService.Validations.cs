﻿// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using CashOverflow.Models.Jobs;
using CashOverflow.Models.Jobs.Exceptions;

namespace CashOverflow.Services.Foundations.Jobs
{
    public partial class JobService
    {
        private void ValidateJobOnModify(Job job)
        {
            ValidateJobNotNull(job);
        }

        private void ValidateJobNotNull(Job job) 
        {
            if(job is null)
            {
                throw new NullJobException();
            }
        }

        private static void ValidateStorageJobExists(Job maybejob, Guid jobId)
        {
            if (maybejob is null)
            {
                throw new NotFoundJobException(jobId);
            }
        }

        private static void ValidateJobId(Guid jobId) =>
           Validate((Rule: IsInvalid(jobId), Parameter: nameof(Job.Id)));

        private static dynamic IsInvalid(Guid jobId) => new
        {
            Condition = jobId == default,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Value is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidJobException = new InvalidJobException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidJobException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidJobException.ThrowIfContainsErrors();
        }
    }
}
