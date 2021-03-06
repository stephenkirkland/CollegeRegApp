﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using University.Users;
using University;

namespace University.Courses
{
    public class Course : ICourse
    {
        #region fields
        public int courseID;
        private string _courseName;
        private int creditHour;
        private string _courseTime;
        public bool courseAvailable = true;
        private List<Student> studentRoster = new List<Student>();
        private int numOfStudents = 0;
        #endregion fields


        #region constructors
        public Course()
        {
        }

        public Course(int ID)
        {
            courseID = ID;
        }

        public Course(int ID, string title, int creditHour, string courseTime, bool courseAvailable, int numOfStudents = 0)
        {
            courseID = ID;
            _courseName = title;
            this.creditHour = creditHour;
            _courseTime = courseTime;
            this.courseAvailable = courseAvailable;
            this.numOfStudents = numOfStudents;
        } // Course constructor
        #endregion constructors


        #region methods
        /// <summary>
        /// declaring the course delegate that closes a course
        /// </summary>
        /// <param name="courseToClose">the course that will be closed</param>
        /// <returns></returns>
        public delegate bool CloseRegistration(Course courseToClose);
        public CloseRegistration cr = null;
        
        /// <summary>
        /// adds a student to the roster and increments the student roster
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public bool AddStudent(Student student)
        {
            SpaceCheck(studentRoster.Count + 1);
            studentRoster.Add(student);
            if (cr != null && isFull == true)
            {
                cr(this);
            }
            return true;
        }

        /// <summary>
        /// adding an entire list of students to the course
        /// </summary>
        /// <param name="roster">list of students</param>
        /// <returns></returns>
        public bool AddStudents(List<Student> roster)
        {
            SpaceCheck(roster.Count + studentRoster.Count);

            foreach (Student item in roster)
            {
                AddStudent(item);
            }
            return true;
        }

        /// <summary>
        /// This method is obsolete because you shouldn't search for a student by their first
        /// name, because there may be multiple students.
        /// </summary>
        /// <param name="firstname"></param>
        /// <returns></returns>
        [Obsolete("method is for LINQ demo")] // example of a directive
        public List<Student> GetStudentByFirstName(string firstname)
        {
            var results = studentRoster.Where(fn => fn.firstname == firstname).ToList();
            return results;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        public IEnumerable<Student> GetStudentByFullName(string fullname)
        {
            var results = studentRoster.Where(fn => fn.Fullname == fullname);
            return results;
        }

        public IEnumerable<Student> GetStudentByFullName(string firstname, string lastname)
        {
            return GetStudentByFullName($"{firstname} {lastname}");
        }

        public void PrintRosterCount()
        {
            Thread.Sleep(1000);
            Console.WriteLine($"number of students: {studentRoster.Count}");
        }

        /// <summary>
        /// supposed to remove a specified student from the course
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public bool RemoveStudent(Student student)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// finds a student's corresponding id and removes that student from the course
        /// </summary>
        /// <param name="id">the student's id who is supposed to be removed</param>
        /// <returns></returns>
        public void RemoveStudent(int id)
        {
            Student s = GetStudentByID(id);
            studentRoster.Remove(s);
        }

        /// <summary>
        /// remove a student by entering their first and last name as parameters
        /// </summary>
        /// <param name="firstname">first name of student</param>
        /// <param name="lastname">last name of student</param>
        /// <returns></returns>
        public bool RemoveStudent(string firstname, string lastname)
        {

            return false;
        }

        /// <summary>
        /// a number will be passed into the parameter and checked to see if there is enough
        /// space in order to chcek to see if one could add more people to the course.
        /// </summary>
        /// <param name="countDractula"></param>
        /// <returns></returns>
        private bool SpaceCheck(int countDractula)
        {
            if (countDractula > Global.maxStudents)
            {
                throw new Exception(Errors.notEnoughError);
            }
            return true;
        }

        public Student GetStudentByID(int id)
        {
            /* The same exact thing as below.
             * var temp = from x in studentRoster 
             * where x.Id == id;
             * select x;
            */

            var student = studentRoster.Where(s => s.Id == id).FirstOrDefault(); // inside the Where() is the Lambda function, where it traverse through the users' ids and returns the id in the function's parameter to the variable s.
            return student;
        }

        private bool CloseCourse(Course courseToClose)
        {
            courseToClose.courseAvailable = false;
            Console.WriteLine($"Registration closed for {courseToClose.CourseName}");
            return true;
        }

        /// <summary>
        /// THREADING: (the following two functions below)
        /// 
        /// returns a list of all the students in the course;
        /// runs simulanteously with the FetchRoster() function;
        /// when the FetchRoster() receives the studentRoster, it then
        /// stores the student roster into a list.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Student>> GetStudentRoster()
        {
            Console.WriteLine("start async");
            Console.WriteLine($"Count before fetch: {studentRoster.Count}");
            var results = await FetchRoster();
            Console.WriteLine($"Count after fetch: {studentRoster.Count}");
            Console.WriteLine("end async");
            return results;
        }

        public Task<List<Student>> FetchRoster()
        {
            return Task.Run(() => { return studentRoster; });
        }

        public void IncrementStudentCount()
        {
            numOfStudents++;
        }

        #endregion methods


        #region properties
        /// <summary>
        /// a property that returns the title of the course
        /// </summary>
        public int CourseID
        {
            get { return courseID; }
            set { courseID = value; }
        }

        public string CourseName
        {
            get{ return _courseName; }
            set{ _courseName = value; }
        }

        /// <summary>
        /// A class can be either 1 or 2 credit hours.
        /// </summary>
        public int CreditHours
        {
            get { return creditHour; }
            set { creditHour = value; }
        }

        public string CourseTime {
            get { return _courseTime; }
            set { _courseTime = value; }
        }

        public bool CourseAvailable
        {
            get { return courseAvailable; }
            set { courseAvailable = value; }
        }

        public bool isAvailable
        {
            get
            {
                return numOfStudents < Global.maxStudents;
            }
        }

        /// <summary>
        /// checks to see if the class is full
        /// </summary>
        public bool isFull
        {
            get
            {
                return studentRoster.Count == Global.maxStudents;
            }
        }

        /// <summary>
        /// counts the number of students in a course
        /// </summary>
        public int RosterCount
        {
            get
            {
                return studentRoster.Count;
            }
        }
        #endregion properties
    }
}
