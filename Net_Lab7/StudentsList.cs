using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net_Lab7
{
    public class StudentsList
    {
        private readonly IFileSaver _fileSaver;
        private readonly IFileLoader _fileLoader;
        private readonly string _filePath;

        public ObservableCollection<Student> Students { get; }
        private List<Student> _allStudents;

        public StudentsList(IFileSaver fileSaver, IFileLoader fileLoader, string filePath)
        {
            _fileSaver = fileSaver;
            _fileLoader = fileLoader;
            _filePath = filePath;

            Students = new ObservableCollection<Student>();
            _allStudents = new List<Student>();
        }

        public void AddStudent(Student student)
        {
            Students.Add(student);
            _allStudents.Add(student);
        }

        public void DeleteStudent(Student student)
        {
            Students.Remove(student);
            _allStudents.Remove(student);
        }

        public void SortByYear()
        {
           // var sortedStudents = new List<Student>(Students);
            //sortedStudents.Sort(new StudentYearComparer());
            //UpdateVisibleCollection(sortedStudents);
            var sortedStudents = Students.OrderBy(s=>s.Year).ToList();
            UpdateVisibleCollection(sortedStudents);
        }

        public void FilterBySchool(string school)
        {
            //var filtered = _allStudents.Where(s => s.School == school).ToList();
            // UpdateVisibleCollection(filtered);
            var filtered = _allStudents.Where(s => s.School.Equals(school, StringComparison.OrdinalIgnoreCase)).OrderBy(s => s.Year).ToList();
            UpdateVisibleCollection(filtered);
        }

        public void ResetFilter()
        {
            UpdateVisibleCollection(_allStudents);
        }

        private void UpdateVisibleCollection(List<Student> newList)
        {
            Students.Clear();
            foreach (var student in newList)
            {
                Students.Add(student);
            }
        }

        public async Task SaveAsync()
        {
            await _fileSaver.SaveAsync(_filePath, _allStudents);
        }

        public async Task LoadAsync()
        {
            var loadedStudents = await _fileLoader.LoadAsync<Student>(_filePath);
            if (loadedStudents != null)
            {
                _allStudents = loadedStudents;
                UpdateVisibleCollection(_allStudents);
            }
        }
        //пошук за частиною імені
        public void SearchStudent(string searchPart)    
        {
            if (string.IsNullOrWhiteSpace(searchPart))
            {
                ResetFilter();
                return;
            }
            string lowerPart = searchPart.ToLowerInvariant();
            var searchResult = _allStudents.Where(s=>s.Name.ToLowerInvariant().Contains(lowerPart)).ToList();
            UpdateVisibleCollection(searchResult);
        }

        // проекцiя! учні з унікальним роком народження
        public void SearchUniqueYear()
        {
            var uniqueYear = _allStudents.Select(s => s.Year).Distinct().OrderBy(a=>a).ToList();
            var studentYr = uniqueYear.Select(a=> new Student("Ім'я", "Школа", 2000) { Year=a}).ToList();
            UpdateVisibleCollection(studentYr);
        }

        //найстарший студент
        public void FindOldestStudent()
        {
            var oldestStudent = _allStudents.OrderBy(s => s.Year).Take(1).ToList();
            UpdateVisibleCollection(oldestStudent);
        }
        // агрегація!  середній рік народження
        public double CalcAverageYear()
        {
            if (_allStudents.Count == 0) return 0;
            return _allStudents.Average(s => s.Year);
        }

        //групування! за школами
        public void GroupBySchool()
        {
            var grouped = _allStudents.GroupBy(s => s.School).OrderByDescending(g=>g.Key)
                .SelectMany(g=>g).ToList();
            UpdateVisibleCollection(grouped);
        }
    }
}
