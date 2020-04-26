// ***********************************************************************
// Author           : the_hunter
// Created          : 04-01-2020
//
// Last Modified By : the_hunter
// Last Modified On : 04-01-2020
// ***********************************************************************

namespace Config.Dialogs
{
    /// <summary>
    ///     Interaction logic for UserPrefsView.xaml
    /// </summary>
    internal partial class UserPrefsView
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UserPrefsView" /> class.
        /// </summary>
        public UserPrefsView(UserPrefsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}