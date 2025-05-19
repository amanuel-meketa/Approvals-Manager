namespace Approvals.Models
{
    public class FormStep
    {
        public string Name { get; set; }
        public string AssignedUserId { get; set; }
        public StepStatus Status { get; set; }
    }
}
