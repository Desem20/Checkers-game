using System;
using System.Drawing;
using System.Windows.Forms;

namespace B18__Ex05
{
    public partial class GameForm : Form
    {
        private const string k_WhitePieces = " O ";
        private const string k_BlackPieces = " X ";
        private const string k_WhiteKing = " U ";
        private const string k_BlackKing = " K ";
        private const string k_EmptyPieces = "   ";
        private const string k_Colon = " : ";
        private GameSettingsForm m_SettingsForm;
        private Board m_BoardOfGame;
        private SquareButton[,] m_BoardButtons;
        private Player m_Player1;
        private Player m_Player2;
        private Point m_SourceOfMove;
        private int m_CountTurns;
        private Label m_LabelOfPlayer1;
        private Label m_LabelOfPlayer2;
        private Player m_CurrentPlayer;

        public GameForm()
        {
            InitializeComponent();
            m_SettingsForm = new GameSettingsForm();
            m_LabelOfPlayer1 = new Label();
            m_LabelOfPlayer2 = new Label();
        }

        private void initializeBoard()
        {
            m_BoardOfGame = new Board(m_SettingsForm.SizeOfBoard);
        }

        private void initialzieButtonsOnBoard()
        {
            m_BoardButtons = new SquareButton[m_BoardOfGame.SizeOfBoard, m_BoardOfGame.SizeOfBoard];
            System.Drawing.Point locationOfSquareButton = new System.Drawing.Point(50, 50);
            for (int indexOfRows = 0; indexOfRows < m_BoardOfGame.SizeOfBoard; indexOfRows++)
            {
                for (int indexOfCols = 0; indexOfCols < m_BoardOfGame.SizeOfBoard; indexOfCols++)
                {
                    m_BoardButtons[indexOfRows, indexOfCols] = new SquareButton(m_BoardOfGame.GetSquare(indexOfRows, indexOfCols), locationOfSquareButton);
                    this.Controls.Add(m_BoardButtons[indexOfRows, indexOfCols]);
                    m_BoardButtons[indexOfRows, indexOfCols].MouseClick += SquareButton_MouseClick;
                    m_BoardOfGame.GetSquare(indexOfRows, indexOfCols).OnSquareIsTakenByPiece();
                    locationOfSquareButton.X += 50;
                }

                locationOfSquareButton.X = 50;
                locationOfSquareButton.Y += 50;
            }
        }

        private void initialziePiecesOnButtonsOnBoard()
        {
            for (int indexOfRows = 0; indexOfRows < m_BoardOfGame.SizeOfBoard; indexOfRows++)
            {
                for (int indexOfCols = 0; indexOfCols < m_BoardOfGame.SizeOfBoard; indexOfCols++)
                {
                    m_BoardOfGame.GetSquare(indexOfRows, indexOfCols).OnSquareIsTakenByPiece();
                }
            }
        }

        private void SquareButton_MouseClick(object sender, MouseEventArgs e)
        {
            SquareButton squareButton = sender as SquareButton;

            if (m_SourceOfMove == null || squareButton.RegularSquare.SquareAlreadyChosen)
            {
                if (m_CountTurns % 2 == 0)
                {
                    m_CurrentPlayer = m_Player1;
                }
                else
                {
                    m_CurrentPlayer = m_Player2;
                }

                m_SourceOfMove = squareButton.SquareLocationInBoard;
                m_CurrentPlayer.PlayerChoseValidPiece(m_SourceOfMove);
                if (!squareButton.RegularSquare.SquareAlreadyChosen)
                {
                    m_SourceOfMove = null;
                }
            }
            else
            {
                bool succeed = getDestinatonAndMakeMove(sender as SquareButton, m_CurrentPlayer);
                if (succeed)
                {
                    if (m_SettingsForm.ComputerPlay && !m_CurrentPlayer.AnotherMoveToEat && !checkIfPlayersHaveNoMovesOrNoPieces())
                    {
                        makeComputerMove();
                    }
                    else if (!m_SettingsForm.ComputerPlay)
                    {
                        if (!m_CurrentPlayer.AnotherMoveToEat)
                        {
                            m_CountTurns++;
                        }
                    }

                    m_SourceOfMove = null;
                    m_CurrentPlayer = null;
                }
                else
                {
                    DialogResult result;
                    result = MessageBox.Show(
@"This isn't a legal move
please make a legal move",
                    "ERROR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }
            }
        }

        private void makeComputerMove()
        {
            m_Player2.PlayerCurrentMove = m_Player2.RandomMove();
            m_Player2.MakeMove();
            while (m_Player2.AnotherMoveToEat)
            {
                m_Player2.PlayerCurrentMove = m_Player2.RandomMove();
                m_Player2.MakeMove();
            }

            if (checkIfPlayersHaveNoMovesOrNoPieces())
            {
                showMessageAfterRound();
            }

            m_CountTurns += 2;
        }

        private bool getDestinatonAndMakeMove(SquareButton i_Destination, Player i_CurrentPlayerTurn)
        {
            Point Destination = i_Destination.SquareLocationInBoard;
            Move currentMove = new Move(m_SourceOfMove, Destination);
            bool isMoveValid = i_CurrentPlayerTurn.CheckIfMoveIsLegal(currentMove);
            i_CurrentPlayerTurn.PlayerCurrentMove = currentMove;
            if (isMoveValid)
            {
                i_CurrentPlayerTurn.MakeMove();
            }

            if (checkIfPlayersHaveNoMovesOrNoPieces())
            {
                showMessageAfterRound();
            }

            return isMoveValid;
        }

        private void initialziePlayersData()
        {
            m_Player1 = new Player(m_SettingsForm.NameOfPlayer1, m_BoardOfGame, "Black");
            m_Player2 = new Player(m_SettingsForm.NameOfPlayer2, m_BoardOfGame, "White");
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            m_SettingsForm.ShowDialog();
            if (m_SettingsForm.SizeOfBoard == 0)
            {
                this.Close();
            }

            initializeBoard();
            initialziePlayersData();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Font = new Font(this.Font, FontStyle.Bold);
            this.Size = new Size((m_SettingsForm.SizeOfBoard * 50) + 100, (m_SettingsForm.SizeOfBoard * 50) + 100);
            m_LabelOfPlayer1.Text = m_Player1.Name + k_Colon + m_Player1.ScoreOfPlayerAllGames;
            m_LabelOfPlayer1.Location = new System.Drawing.Point(80, 20);
            m_LabelOfPlayer2.Location = new System.Drawing.Point(120, 20);
            m_LabelOfPlayer2.Text = m_Player2.Name + k_Colon + m_Player2.ScoreOfPlayerAllGames;
            m_LabelOfPlayer2.Location = new System.Drawing.Point(m_LabelOfPlayer1.Location.X + (50 * (m_SettingsForm.SizeOfBoard / 2)), m_LabelOfPlayer2.Location.Y);
            Controls.Add(m_LabelOfPlayer1);
            Controls.Add(m_LabelOfPlayer2);
            initialzieButtonsOnBoard();
        }

        private void showMessageAfterRound()
        {
            updateScoresOfPlayersInCurrentRound();

            bool isTie = checkIfGameAndInTie();
            DialogResult result;
            if (isTie)
            {
                string messageTie = @"Tie!
Antoher Round?";
                result = MessageBox.Show(messageTie, "Damka", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            else
            {
                string winnerName = checkWhoWonTheRound();
                string messageWin = @"{0} Won!
Antoher Round?";
                messageWin = string.Format(messageWin, winnerName);
                result = MessageBox.Show(messageWin, "Damka", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }

            checkIfPlayAnotherRound(result);
        }

        private void showNewRoundButtons()
        {
            for (int indexOfRows = 0; indexOfRows < m_BoardOfGame.SizeOfBoard; indexOfRows++)
            {
                for (int indexOfCols = 0; indexOfCols < m_BoardOfGame.SizeOfBoard; indexOfCols++)
                {
                    string a = m_BoardButtons[indexOfRows, indexOfCols].Text;
                    m_BoardButtons[indexOfRows, indexOfCols].Show();
                }
            }
        }

        private void checkIfPlayAnotherRound(DialogResult result)
        {
            if (result == DialogResult.No)
            {
                this.Close();
            }
            else
            {
                m_BoardOfGame.SetBoardForNextRound();
                this.initialziePiecesOnButtonsOnBoard();
                updatePlayersScoresAfterRound();
                showNewRoundButtons();
                m_SourceOfMove = null;
                m_LabelOfPlayer1.Text = m_Player1.Name + k_Colon + m_Player1.ScoreOfPlayerAllGames;
                m_LabelOfPlayer2.Text = m_Player2.Name + k_Colon + m_Player2.ScoreOfPlayerAllGames;
            }
        }

        private string checkWhoWonTheRound()
        {
            string winnerName;
            if (m_Player1.ScoreOfPlayerInCurrentGame > m_Player2.ScoreOfPlayerInCurrentGame)
            {
                winnerName = m_Player1.Name;
            }
            else
            {
                winnerName = m_Player2.Name;
            }

            return winnerName;
        }

        private bool checkIfGameAndInTie()
        {
            return m_Player1.CheckIfPlayerHasNoValidMoves() && m_Player2.CheckIfPlayerHasNoValidMoves() && m_Player1.PiecesOfPlayer.Count == m_Player2.PiecesOfPlayer.Count;
        }

        private void updatePlayersScoresAfterRound()
        {
            if (m_Player1.ScoreOfPlayerInCurrentGame >= m_Player2.ScoreOfPlayerInCurrentGame)
            {
                m_Player1.ScoreOfPlayerAllGames += m_Player1.ScoreOfPlayerInCurrentGame - m_Player2.ScoreOfPlayerInCurrentGame;
            }
            else
            {
                m_Player2.ScoreOfPlayerAllGames += m_Player2.ScoreOfPlayerInCurrentGame - m_Player1.ScoreOfPlayerInCurrentGame;
            }
        }

        private void updateScoresOfPlayersInCurrentRound()
        {
            m_Player1.UpdateScoreOfPlayerInCurrentRound();
            m_Player2.UpdateScoreOfPlayerInCurrentRound();
        }

        private bool checkIfPlayersHaveNoMovesOrNoPieces()
        {
            return m_Player1.CheckIfPlayerHasNoValidMoves() || m_Player2.CheckIfPlayerHasNoValidMoves();
        }
    }
}