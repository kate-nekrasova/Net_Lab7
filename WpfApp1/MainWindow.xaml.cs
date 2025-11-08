using System;
using System.IO;
using System.Windows;
using Net_Lab7;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private StudentsList _studentsList;

        public MainWindow()
        {
            InitializeComponent();

            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = Path.Combine(documentsPath, "students.json");

            _studentsList = new StudentsList(new JsonFileWriter(), new JsonFileReader(), filePath);
            StudentsDataGrid.ItemsSource = _studentsList.Students;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var student = new Student(
                    NameTextBox.Text,
                    SchoolTextBox.Text,
                    int.Parse(YearTextBox.Text)
                );

                _studentsList.AddStudent(student);

                NameTextBox.Clear();
                YearTextBox.Clear();
                SchoolTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsDataGrid.SelectedItem is Student selected)
                _studentsList.DeleteStudent(selected);
        }

        private void SortByYearButton_Click(object sender, RoutedEventArgs e)
        {
            _studentsList.SortByYear();
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            string school = FilterSchoolTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(school))
                _studentsList.ResetFilter();
            else
                _studentsList.FilterBySchool(school);
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            await _studentsList.SaveAsync();
            MessageBox.Show("Дані успішно збережено.", "Збереження", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            await _studentsList.LoadAsync();
            MessageBox.Show("Дані успішно завантажено.", "Завантаження", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                _studentsList.ResetFilter();
            }
            else
            {
                _studentsList.SearchStudent(SearchTextBox.Text);
            }
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            _studentsList.SearchStudent(SearchTextBox.Text);
        }

        private void UniqueYearButton_Click(object sender, RoutedEventArgs e)
        {
            var uniqueYears = _studentsList.Students.Select(s => s.Year).Distinct().OrderBy(y => y).ToList();
            MessageBox.Show("Унікальні роки: " + string.Join(", ", uniqueYears), "Унікальні роки", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OldestStudentButton_Click(object sender, RoutedEventArgs e)
        {
            var oldest = _studentsList.Students.OrderBy(s => s.Year).FirstOrDefault();
            if (oldest != null)
                MessageBox.Show($"Найстарший студент: {oldest.Name}, рік народження: {oldest.Year}", "Найстарший студент", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AverageYearButton_Click(object sender, RoutedEventArgs e)
        {
            if (_studentsList.Students.Count > 0)
            {
                double average = _studentsList.Students.Average(s => s.Year);
                MessageBox.Show($"Середній рік народження: {average:F2}", "Середній рік", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Список студентів порожній.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void GroupBySchoolButton_Click(object sender, RoutedEventArgs e)
        {
            var groups = _studentsList.Students.GroupBy(s => s.School)
                                               .Select(g => $"{g.Key}: {g.Count()} студентів");
            MessageBox.Show(string.Join(Environment.NewLine, groups), "Групування за школою", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
