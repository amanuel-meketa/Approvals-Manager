using Approvals.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Approvals.Controllers
{
    public class FormWorkflowController : Controller
    {
        // Simple in-memory store for demonstration
        private static readonly List<FormWorkflow> _workflows = new List<FormWorkflow>();

        public IActionResult Index()
        {
            return View(_workflows);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string title)
        {
            var workflow = new FormWorkflow
            {
                Id = Guid.NewGuid(),
                Title = title,
                Status = FormStatus.Submitted,
                Steps = new List<FormStep>
                {
                    new FormStep { Name = "Title Submission", AssignedUserId = "user1", Status = StepStatus.Pending },
                    new FormStep { Name = "Proposal Submission", AssignedUserId = "user2", Status = StepStatus.NotStarted },
                    new FormStep { Name = "Final Submission", AssignedUserId = "user3", Status = StepStatus.NotStarted }
                },
                CurrentStepIndex = 0
            };
            _workflows.Add(workflow);
            return RedirectToAction("Details", new { id = workflow.Id });
        }

        public IActionResult Details(Guid id)
        {
            var workflow = _workflows.FirstOrDefault(w => w.Id == id);
            if (workflow == null) return NotFound();
            return View(workflow);
        }

        [HttpPost]
        public IActionResult ApproveStep(Guid id)
        {
            var workflow = _workflows.FirstOrDefault(w => w.Id == id);
            if (workflow == null) return NotFound();

            SubmitStep(workflow, true);
            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        public IActionResult DeclineStep(Guid id)
        {
            var workflow = _workflows.FirstOrDefault(w => w.Id == id);
            if (workflow == null) return NotFound();

            SubmitStep(workflow, false);
            return RedirectToAction("Details", new { id });
        }

        private void SubmitStep(FormWorkflow workflow, bool approve)
        {
            var idx = workflow.CurrentStepIndex;
            var step = workflow.Steps[idx];
            if (step.Status == StepStatus.Approved || step.Status == StepStatus.Declined)
                return;

            step.Status = approve ? StepStatus.Approved : StepStatus.Declined;

            if (!approve)
            {
                workflow.Status = FormStatus.Declined;
                return;
            }

            if (idx == workflow.Steps.Count - 1)
            {
                workflow.Status = FormStatus.Approved;
            }
            else
            {
                workflow.CurrentStepIndex++;
                workflow.Status = FormStatus.Pending;
                workflow.Steps[workflow.CurrentStepIndex].Status = StepStatus.Pending;
            }
        }
    }
}