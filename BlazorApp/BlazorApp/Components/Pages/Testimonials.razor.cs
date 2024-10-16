using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BlazorApp.Pages // Replace with the actual namespace
{
    public partial class Testimonials : ComponentBase
    {
        // FAQ Item class
        public class FAQItem
        {
            public int Id { get; set; }
            public string Question { get; set; } = string.Empty;
            public string Answer { get; set; } = string.Empty;
            public bool IsExpanded { get; set; } = false;
        }

        // FAQ items list
        protected List<FAQItem> FaqItems = new List<FAQItem>
        {
            new FAQItem
            {
                Id = 1,
                Question = "What does ABA stand for?",
                Answer = "ABA is an acronym for Applied Behavior Analysis, also referred to as Behavioral Engineering. The ABA method is made up of scientifically progressive techniques used to improve or correct certain types of behavior. Working with children within the autism spectrum, the therapy has proven success in managing problematic behavior while developing social and academic skills."
            },
            new FAQItem
            {
                Id = 2,
                Question = "What does BCBA stand for?",
                Answer = "BCBA stands for Board Certified Behavior Analyst..."
            },
            new FAQItem
            {
                Id = 3,
                Question = "Can you give me specific examples of behavior that ABA therapy focuses on?",
                Answer = "ABA therapy can help children improve social skills, enhance communication, and reduce problematic behaviors..."
            },
            new FAQItem
            {
                Id = 4,
                Question = "Is there an age-appropriate time to begin ABA therapy?",
                Answer = "ABA therapy can be beneficial for children of various ages, depending on their needs..."
            },
            new FAQItem
            {
                Id = 5,
                Question = "How do I know if my child needs ABA therapy?",
                Answer = "Consulting with a developmental specialist or a BCBA can help determine whether ABA therapy is right for your child..."
            }
        };

        // Method to toggle FAQ visibility
        protected void ToggleFAQ(int id)
        {
            var faq = FaqItems.Find(f => f.Id == id);
            if (faq != null)
            {
                faq.IsExpanded = !faq.IsExpanded;
            }
        }
    }
}