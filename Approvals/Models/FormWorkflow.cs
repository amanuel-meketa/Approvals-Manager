using System.Collections.Generic;
using System;

namespace Approvals.Models
{
    public class FormWorkflow
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public FormStatus Status { get; set; }
        public List<FormStep> Steps { get; set; }
        public int CurrentStepIndex { get; set; }
    }
}
