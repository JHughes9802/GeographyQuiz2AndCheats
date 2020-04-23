using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeographyQuiz2
{
    public partial class Form1 : Form
    {
        // Instantiation of the Questions and Answers lists
        private readonly List<string> Questions = new List<string> {"What is the state capital of California?",
            "What is the tallest mountain on land in the world?",
            "Which country has the furthest south extent?"};

        private readonly List<string> Answers = new List<string> {"Sacramento",
            "Everest",
            "Chile"};

        // I made all these variables global because I don't have a good way to pass these between methods
        private int QuestionNumber = -1; // Counter to keep track of the current question/answer
        private int Score = 0;
        private int TimesCheated = 0; // A counter to display at the end how many questions the user cheated on
        private bool CheatClicked = false; // This is to keep track for each question the user cheats on
        private StringBuilder IncorrectAnswersBuilder = new StringBuilder();
        DateTime startTime = DateTime.Now; // This gets the start time right when the form is opened

        public Form1()
        {
            InitializeComponent();
            ShowNextQuestion();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblScore.Text = $"Score: {Score}"; // This porperly sets lblScore on form load
        }

        private void ShowNextQuestion()
        {
            QuestionNumber++;

            // I considered using a foreach loop, but couldn't manage to get it to work how I wanted
            if (QuestionNumber < Questions.Count)
            {
                lblQuestion.Text = Questions[QuestionNumber];
                // QuestionNumber++ can fit here if it starts at 0 and the below statement has + 1 removed, but it looks strange

                // I added this so the user can know how far they are in the quiz - it's nice for long quizzes
                lblQuestionNumber.Text = $"Question: {QuestionNumber + 1}/{Questions.Count}";

                // This is basic QoL (Quality of Life) that I like adding when possible
                txtAnswer.Clear();
                txtAnswer.Focus();
            }

            else
            {
                btnSubmitAnswer.Enabled = false;
                btnCheat.Enabled = false;

                EndMessage();

                Close();
            }
        }

        /* I originally created the QuizOver method because there was too much unrelated 
         * code in ShowNextQuestion, but now the EndMessage method handles everything

        private void QuizOver()
        {
            /* I use an if/else because it's improper to have the line
             * "Incorrect Answers" display if they got them all correct */
            /* I don't like having 4 different chunks of code with barely different MessageBox
             * statements, but I don't think there's a simple solution given what we've been taught.
             * What's worse, this would be 6 statements if I accounted for "else if (TimesCheated == 1)"
             * and changed the Message to say "You cheated {TimesCheated} time" */

             /* The commented code below is the code I used to have, but I felt a need to improve it

            if (Score == Questions.Count)
            {
                if (TimesCheated == 0)
                {
                    MessageBox.Show($"Your score is {Score}" +
                        $"\nIt took {minutes} minutes and {seconds} seconds to finish",
                        "Quiz Over!");
                }

                else
                {
                    MessageBox.Show($"Your score is {Score}" +
                        $"\nIt took {minutes} minutes and {seconds} seconds to finish" +
                        $"\nYou cheated {TimesCheated} times",
                        "Quiz Over!");
                }
            } */

            /* This formatting works fine since it's a short quiz, but it'd need to be changed when
             * there's more than 5 questions because the message box could get cut off */
             /*
            else
            {
                if (TimesCheated == 0)
                {
                    string incorrectAnswers = IncorrectAnswersBuilder.ToString();
                    MessageBox.Show($"Your score is {Score}" +
                        $"\nIt took {minutes} minutes and {seconds} seconds to finish" +
                        $"\n\nIncorrect Answers {incorrectAnswers}",
                        "Quiz Over!");
                }

                else
                {
                    string incorrectAnswers = IncorrectAnswersBuilder.ToString();
                    MessageBox.Show($"Your score is {Score}" +
                        $"\nIt took {minutes} minutes and {seconds} seconds to finish" +
                        $"\nYou cheated {TimesCheated} times" +
                        $"\n\nIncorrect Answers {incorrectAnswers}",
                        "Quiz Over!");
                }
            }
        } */

        /* The code I used to build the message for the final MessageBox. While it's more lines
         * of code than above, it accounts for more scenarios and is easier to adjust if I were
         * to modify the code. Also, I feel it's easier to read on the coding side */
        private void EndMessage()
        {
            // Given how short the quiz is, I felt it proper to include seconds
            TimeSpan timeTaken = DateTime.Now - startTime;
            int minutes = timeTaken.Minutes;
            int seconds = timeTaken.Seconds;

            StringBuilder EndMessageBuilder = new StringBuilder();

            EndMessageBuilder.Append("Your score is ");
            EndMessageBuilder.Append(Score);
            EndMessageBuilder.Append("\nYou took ");

            /* I decided to account for words referring to a single digit to lack the plural "s"
             * and, given the length of the quiz, only adding the minute(s) if it's applicable
             * because I knew I could. As simple as this is, I find joy in attention to detail */
            if (minutes > 0)
            {
                EndMessageBuilder.Append(minutes);

                if (minutes == 1)
                {
                    EndMessageBuilder.Append(" minute ");
                }

                else
                {
                    EndMessageBuilder.Append(" minutes ");
                }

                EndMessageBuilder.Append("and ");
            }

            EndMessageBuilder.Append(seconds);

            if (seconds == 1)
            {
                EndMessageBuilder.Append(" second to finish");
            }

            else
            {
                EndMessageBuilder.Append(" seconds to finish");
            }

            if (TimesCheated > 0)
            {
                EndMessageBuilder.Append("\nYou cheated ");
                EndMessageBuilder.Append(TimesCheated);

                if (TimesCheated == 1)
                {
                    EndMessageBuilder.Append(" time");
                }

                else
                {
                    EndMessageBuilder.Append(" times");
                }
            }

            if (Score < Questions.Count)
            {
                EndMessageBuilder.Append("\n\nIncorrect answers");
                EndMessageBuilder.Append(IncorrectAnswersBuilder); // I didn't know I could so easily combine MessageBuilders
            }

            MessageBox.Show(EndMessageBuilder.ToString(), "Quiz Over!");
        }
        
        private void CheckAnswer()
        {
            string answer = txtAnswer.Text.Trim(); // Included the trim function to increase accuracy
            
            /* I know I could do this with a sorted list, but I wanted to see if I could make things
             * work instead with two separate lists. I'd still prefer a sorted list mainly because
             * I can easily see if question and answer are at the same index if there's many questions */
            // I do Length + 5 to attempt to in case the user adds " city" or "Mt. "
            // I'm curious what you'd do/suggest to check for correct answers
            if (answer.Contains(Answers[QuestionNumber]) &&
                answer.Length <= Answers[QuestionNumber].Length + 5)
            {
                MessageBox.Show("Correct!", "Result");
                Score++;

                // I added this so the user can know their total score during the quiz
                lblScore.Text = $"Score: {Score}";
            }

            else
            {
                MessageBox.Show("Incorrect", "Result");

                // I decided to use a MessageBuilder for this quiz app, I like how the output came out
                IncorrectAnswersBuilder.Append("\n\nQuestion: ");
                IncorrectAnswersBuilder.Append(Questions[QuestionNumber]);
                IncorrectAnswersBuilder.Append("\nYour answer: ");
                IncorrectAnswersBuilder.Append(answer);
                IncorrectAnswersBuilder.Append("\nCorrect answer: ");
                IncorrectAnswersBuilder.Append(Answers[QuestionNumber]);
            }
        }

        // A simple method to check if the user clicked btnCheat before clicking btnSubmitAnswer
        private void CheckIfCheated()
        {
            if (CheatClicked)
            {
                TimesCheated++;
                CheatClicked = false;
            }
        }

        private void btnSubmitAnswer_Click(object sender, EventArgs e)
        {
            /* This is to ensure the user entered an answer before checking if their answer
             * is correct because it'd be improper to tell them they got it wrong if they
             * accidentally pressed the enter key before entering anything */
            if (String.IsNullOrWhiteSpace(txtAnswer.Text))
            {
                MessageBox.Show("Please enter an answer", "Error");
                txtAnswer.Focus();
            }

            else
            {
                CheckAnswer();
                CheckIfCheated();
                ShowNextQuestion();
            }
        }

        private void btnCheat_Click(object sender, EventArgs e)
        {
            // My thought process for adding the cheat Form/Button
            // Needs to send Answers[QuestionNumber]
            // Needs to receive whether or not they clicked "I'm Lazy"
            // If so, txtAnswer.Text = Answers[QuestionNumber];
            CheatClicked = true;

            CheatForm frmCheatForm = new CheatForm();
            frmCheatForm.Tag = Answers[QuestionNumber];
            DialogResult cheatFormResult = frmCheatForm.ShowDialog();
            txtAnswer.Focus();

            if (cheatFormResult == DialogResult.OK)
            {
                txtAnswer.Text = Answers[QuestionNumber];
                btnSubmitAnswer.Focus(); // I could arguably have this click btnSubmitAnswer for the user instead
            }
        }
    }
}
