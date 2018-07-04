using FluentNHibernate.Mapping;

namespace Logic.Students
{
    public class DisenrollmentMap : ClassMap<Disenrollment>
    {
        public DisenrollmentMap()
        {
            Id(x => x.Id);

            Map(x => x.DateTime);
            Map(x => x.Comment);

            References(x => x.Student);
            References(x => x.Course);
        }
    }
}
