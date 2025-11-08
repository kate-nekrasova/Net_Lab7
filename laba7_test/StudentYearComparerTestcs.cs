using Net_Lab7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba6_tests
{
    public class StudentYearComparerTest
    {
        private StudentYearComparer comparer;

        [TestInitialize]
        public void Setup()
        {
            comparer = new StudentYearComparer();
        }

        [TestMethod]
        public void Compare_DifferentYears_CorrectOrder()
        {
            var student1 = new Student("Alice", "School 1", 2000);
            var student2 = new Student("Bob", "School 2", 2010);

            Assert.IsTrue(comparer.Compare(student1, student2) < 0);
            Assert.IsTrue(comparer.Compare(student2, student1) > 0);
        }

        [TestMethod]
        public void Compare_SameYears_ReturnsZero()
        {
            var student1 = new Student("Alice", "School 1", 2005);
            var student2 = new Student("Bob", "School 2", 2005);

            Assert.AreEqual(0, comparer.Compare(student1, student2));
        }

        [TestMethod]
        public void Compare_FirstStudentYearZero_ReturnsNegative()
        {
            var student1 = new Student("Alice", "School 1", 0);
            var student2 = new Student("Bob", "School 2", 2000);

            Assert.IsTrue(comparer.Compare(student1, student2) < 0);
        }

        [TestMethod]
        public void Compare_SecondStudentYearZero_ReturnsPositive()
        {
            var student1 = new Student("Alice", "School 1", 2000);
            var student2 = new Student("Bob", "School 2", 0);

            Assert.IsTrue(comparer.Compare(student1, student2) > 0);
        }

        [TestMethod]
        public void Compare_BothYearsZero_ReturnsZero()
        {
            var student1 = new Student("Alice", "School 1", 0);
            var student2 = new Student("Bob", "School 2", 0);

            Assert.AreEqual(0, comparer.Compare(student1, student2));
        }

        [TestMethod]
        public void Compare_FirstStudentNull_ReturnsNegativeOne()
        {
            Student student1 = null;
            var student2 = new Student("Bob", "School 2", 2000);

            Assert.AreEqual(-1, comparer.Compare(student1, student2));
        }

        [TestMethod]
        public void Compare_SecondStudentNull_ReturnsPositiveOne()
        {
            var student1 = new Student("Alice", "School 1", 2000);
            Student student2 = null;

            Assert.AreEqual(1, comparer.Compare(student1, student2));
        }

        [TestMethod]
        public void Compare_BothStudentsNull_ReturnsZero()
        {
            Student student1 = null;
            Student student2 = null;

            Assert.AreEqual(0, comparer.Compare(student1, student2));
        }
    }
}
