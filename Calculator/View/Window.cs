using Calculator.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator.View
{
    public partial class Window : Form
    {
        private readonly Model.Model _model;
        private string _current = "0";

        public Window(Model.Model model)
        {
            InitializeComponent();

            _model = model;

            Digit1.Click += (s, e) => AddDigit('1');
            Digit2.Click += (s, e) => AddDigit('2');
            Digit3.Click += (s, e) => AddDigit('3');
            Digit4.Click += (s, e) => AddDigit('4');
            Digit5.Click += (s, e) => AddDigit('5');
            Digit6.Click += (s, e) => AddDigit('6');
            Digit7.Click += (s, e) => AddDigit('7');
            Digit8.Click += (s, e) => AddDigit('8');
            Digit9.Click += (s, e) => AddDigit('9');
            Digit0.Click += (s, e) => AddDigit('0');

            AddButton.Click += (s, e) => AddOperator(Operation.Addition);
            SubButton.Click += (s, e) => AddOperator(Operation.Subtraction);
            MultiButton.Click += (s, e) => AddOperator(Operation.Multiplication);
            DivButton.Click += (s, e) => AddOperator(Operation.Division);
            ResultButton.Click += (s, e) => Finish();

            PointButton.Click += (s, e) =>
            {
                if (!_current.Contains('.'))
                {
                    _current += '.';
                }
                ShowResult();
            };
        }

        private void Finish()
        {
            _model.Push(double.Parse(_current, CultureInfo.InvariantCulture));
            ResetCurrent(_model.GetResult());
            _model.Clear();
            ShowResult();
        }

        private void AddOperator(Operation op)
        {
            _model.Push(double.Parse(_current, CultureInfo.InvariantCulture));
            _model.Push(op);
            ResetCurrent(0);
            ShowResult();
        }

        void ShowResult()
        {
            var stringTokens = _model.GetTokens().Select(
                tok => RenderToken(tok));
            resultLabel.Text = string.Join(" ", stringTokens) + " " + _current;
        }

        void AddDigit(char digit)
        {
            if (_current == "0")
            {
                _current = "";
            }
            _current += digit;
            ShowResult();
        }

        void ResetCurrent(double value)
        {
            _current = value.ToString();
        }

        static string RenderToken(Model.Model.Token token)
        {
            if (token.Operand.HasValue)
            {
                return token.Operand.ToString();
            }

            switch (token.Operation.Value)
            {
                case Operation.Addition:
                    return "+";
                case Operation.Subtraction:
                    return "-";
                case Operation.Multiplication:
                    return "*";
                case Operation.Division:
                    return "/";
                case Operation.Percent:
                    return "%";
            }

            throw new ArgumentOutOfRangeException();
        }

    }
}
