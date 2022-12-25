using System.Collections.Generic;

namespace VLUTUTORS.Models
{
    public class TutorListModel
    {
        public List<TutorViewModel> awaitTutors { get; set; }
        public List<TutorViewModel> approvedTutors { get; set; }
    }
}
