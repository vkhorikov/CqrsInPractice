using FluentNHibernate.Mapping;

namespace Logic.Students
{
    public class EnrollmentMap : ClassMap<Enrollment>
    {
        public EnrollmentMap()
        {
            Id(x => x.Id);

            Map(x => x.Grade).CustomType<int>();

            References(x => x.Student);
            References(x => x.Course);
        }
    }
}
