namespace BoardGameFramework{
    public class GameUtils{

        public static bool PlaceNumber(int?[,] boardgrid, int row, int col, int value)
        {
            if (row < 0 || row >= boardgrid.GetLength(0) || col < 0 || col >= boardgrid.GetLength(1)){
                return false;
            }

            if (boardgrid[row, col] != null){
                return false;
            }  

            boardgrid[row, col] = value;
            return true;
        }

        public static bool CheckWin(int? [,] boardGrid, int boardsize)
        {
            int magicSum = boardsize * (boardsize * boardsize + 1) / 2;

            for(int i = 0; i < boardsize; i++){

                int sum = 0;
                bool complete = true;

                for(int j = 0; j < boardsize; j++){
                    if(boardGrid[i,j] == null){
                        complete = false;
                        break;
                    }
                    int cellValue = boardGrid[i, j]!.Value;
                    sum += cellValue;
                }

                if(complete && sum == magicSum){
                    return true;
                }
            }

            for(int j = 0; j < boardsize; j++){

                int sum = 0;
                bool complete = true;

                for(int i = 0; i < boardsize; i++){
                    if(boardGrid[i,j] == null){
                        complete = false;
                        break;
                    }
                    int cellValue = boardGrid[i, j]!.Value;
                    sum += cellValue;
                }

                if(complete && sum == magicSum){
                    return true;
                }
            }

            //check fro diagonal
            int diagSum1 = 0;
            bool diagComplete = true;

            for(int i = 0; i < boardsize; i++){
                if(boardGrid[i, i] == null){
                    diagComplete = false;
                    break;
                }
                int cellValue = boardGrid[i, i]!.Value;
                diagSum1 += cellValue;
            }
            if(diagComplete && diagSum1 == magicSum){
                return true;
            }

            //check for other diagonal
            int diagSum2 = 0;
            bool diagComplete2 = true;

            for(int i = 0; i < boardsize; i++){
                if(boardGrid[i, i] == null){
                    diagComplete2 = false;
                    break;
                }
                int cellValue = boardGrid[i, i]!.Value;
                diagSum2 += cellValue;
            }
            if(diagComplete2 && diagSum2 == magicSum){
                return true;
            }

            return false;
        }

        public static bool isBoardFull(int?[,] boardGrid)
        {
            foreach (var cell in boardGrid)
            {
                if (cell == null){
                    return false;
                }
            }
            return true;
        }

        public static bool CheckFiveInARow(int?[,] board, int size, int symbol)
        {
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    if (board[row, col] == symbol)
                    {
                        if (CheckDirection(board, row, col, symbol, 1, 0) ||  // vertical
                            CheckDirection(board, row, col, symbol, 0, 1) ||  // horizontal
                            CheckDirection(board, row, col, symbol, 1, 1) ||  // diagonal /
                            CheckDirection(board, row, col, symbol, 1, -1))   // diagonal \
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private static bool CheckDirection(int?[,] board, int row, int col, int symbol, int dRow, int dCol)
        {
            int count = 0;
            int size = board.GetLength(0);

            for (int i = 0; i < 5; i++)
            {
                int r = row + i * dRow;
                int c = col + i * dCol;

                if (r < 0 || c < 0 || r >= size || c >= size || board[r, c] != symbol)
                    return false;

                count++;
            }

            return count == 5;
        }
    }
}