namespace Logic.Students
{
    public class Enrollment : Entity
    {
        public virtual Student Student { get; protected set; }
        public virtual Course Course { get; protected set; }
        public virtual Grade Grade { get; protected set; }

        protected Enrollment()
        {
        }

        public Enrollment(Student student, Course course, Grade grade)
            : this()
        {
            Student = student;
            Course = course;
            Grade = grade;
        }

        public virtual void Update(Course course, Grade grade)
        {
            Course = course;
            Grade = grade;
        }
    }

    public enum Grade
    {
        A = 1,
        B = 2,
        C = 3,
        D = 4,
        F = 5
    }
}
