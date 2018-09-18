using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Logic.Dtos;
using Logic.Students;
using Logic.Utils;

namespace Logic.AppServices
{
    public sealed class GetListQuery : IQuery<List<StudentDto>>
    {
        public string EnrolledIn { get; }
        public int? NumberOfCourses { get; }

        public GetListQuery(string enrolledIn, int? numberOfCourses)
        {
            EnrolledIn = enrolledIn;
            NumberOfCourses = numberOfCourses;
        }

        internal sealed class GetListQueryHandler : IQueryHandler<GetListQuery, List<StudentDto>>
        {
            private readonly ConnectionString _connectionString;

            public GetListQueryHandler(ConnectionString connectionString)
            {
                _connectionString = connectionString;
            }

            public List<StudentDto> Handle(GetListQuery query)
            {
                string sql = @"
                    SELECT s.*, e.Grade, c.Name CourseName, c.Credits
                    FROM dbo.Student s
                    LEFT JOIN (
	                    SELECT e.StudentID, COUNT(*) Number
	                    FROM dbo.Enrollment e
	                    GROUP BY e.StudentID) t ON s.StudentID = t.StudentID
                    LEFT JOIN dbo.Enrollment e ON e.StudentID = s.StudentID
                    LEFT JOIN dbo.Course c ON e.CourseID = c.CourseID
                    WHERE (c.Name = @Course OR @Course IS NULL)
                        AND (ISNULL(t.Number, 0) = @Number OR @Number IS NULL)
                    ORDER BY s.StudentID ASC";

                using (SqlConnection connection = new SqlConnection(_connectionString.Value))
                {
                    List<StudentInDB> students = connection
                        .Query<StudentInDB>(sql, new
                        {
                            Course = query.EnrolledIn,
                            Number = query.NumberOfCourses
                        })
                        .ToList();

                    List<long> ids = students
                        .GroupBy(x => x.StudentID)
                        .Select(x => x.Key)
                        .ToList();

                    var result = new List<StudentDto>();

                    foreach (long id in ids)
                    {
                        List<StudentInDB> data = students
                            .Where(x => x.StudentID == id)
                            .ToList();

                        var dto = new StudentDto
                        {
                            Id = data[0].StudentID,
                            Name = data[0].Name,
                            Email = data[0].Email,
                            Course1 = data[0].CourseName,
                            Course1Credits = data[0].Credits,
                            Course1Grade = data[0]?.Grade.ToString()
                        };
                        if (data.Count > 1)
                        {
                            dto.Course2 = data[1].CourseName;
                            dto.Course2Credits = data[1].Credits;
                            dto.Course2Grade = data[1]?.Grade.ToString();
                        }

                        result.Add(dto);
                    }

                    return result;
                }
            }

            private class StudentInDB
            {
                public readonly long StudentID;
                public readonly string Name;
                public readonly string Email;
                public readonly Grade? Grade;
                public readonly string CourseName;
                public readonly int? Credits;

                public StudentInDB(long studentId, string name, string email,
                    Grade? grade, string courseName, int? credits)
                {
                    StudentID = studentId;
                    Name = name;
                    Email = email;
                    Grade = grade;
                    CourseName = courseName;
                    Credits = credits;
                }
            }
        }
    }
}