using System.Windows;

namespace wFlashcards
{
    public partial class MainWindow : Window
    {
        private FlashcardsViewModel _flashcardsViewModel;

        public MainWindow()
        {
            _flashcardsViewModel = new FlashcardsViewModel();
            this.DataContext = _flashcardsViewModel;
            InitializeComponent();
        }

        private void ShowAnswer_OnClick(object sender, RoutedEventArgs e)
        {
            _flashcardsViewModel.ShowAnswer();
        }

        private void IncorrectButtton_OnClick(object sender, RoutedEventArgs e)
        {
            _flashcardsViewModel.IncorrectSelected();
        }

        private void CorrectButton_OnClick(object sender, RoutedEventArgs e)
        {
            _flashcardsViewModel.CorrectSelected();
        }
    }
}
