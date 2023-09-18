using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using log4net;
using log4net.Config;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class BoardFacade
    {
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<string, List<Board>> boards;
        private UserFacade users;
        private BoardDAO boardd;
        private UserBoardDAO ubd;
        private ColumnDAO columnDao;
        internal TaskDAO taskDao;
        private int boardId;

        public BoardFacade(UserFacade uf)
        {
            ubd = new UserBoardDAO();
            boardd = new BoardDAO();
            this.boardId = 0;
            boards = new Dictionary<string, List<Board>>();
            users = uf;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            columnDao = new ColumnDAO();
            taskDao = new TaskDAO();
        }

        /// <summary>
        /// Getting a board of given user and given board name
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Board GetBoard(string email, string boardName)
        {
            if (!users.UserExists(email))
            {
                throw new Exception("user " + email + " is not exists!");
            }

            if (!boards.ContainsKey(email))
            {
                throw new Exception("user " + email + " does not has anyboards");
            }

            if (!users.IsLoggedIn(email))
                throw new Exception("user " + email + " is not logged in!");
            foreach (Board board in boards[email])
            {
                if (boardName == board.getName())
                {
                    return board;
                }
            }

            throw new ArgumentException("There's no board with a given name ");
        }

        /// <summary>
        /// Getting a board of given user and given board id
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Board GetBoard(long id)
        {
            foreach (string email in boards.Keys)
            {
                foreach (Board board in boards[email])
                {
                    if (id == board.GetId())
                    {
                        return board;
                    }
                }
            }

            throw new ArgumentException("There's no board with a given id");
        }

        /// <summary>
        /// Creating a board for the given user but without a limit of columns
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool CreateBoard(string email, string boardName)
        {
            if (boardName == null || boardName.Length < 1) throw new Exception("board name cannot be empty!");
            if (!users.UserExists(email))
            {
                throw new Exception("user " + email + " is not exists!");
            }
            if (!users.IsLoggedIn(email)) 
                throw new Exception("user " + email + " is not logged in!");
            if (!boards.ContainsKey(email))
            {
                boards.Add(email, new List<Board>());
            }
            List<Board> b = boards[email];
            foreach (Board board in b)
            {
                if(board.GetOwner().Equals(email) && boardId == board.GetId())
                {
                    throw new Exception("a board with such id already exist!");
                }
                if (email == board.GetOwner() && board.getName().Equals(boardName))
                {
                    throw new Exception("a board with such name already exist!");
                }
            }
            boards[email].Add(new Board(boardName, email, boardId));
            
            if (!boardd.Insert(new BoardDTO(boardName, email, boardId, -1, -1, -1)))
            {
                throw new Exception("cannot add board");
            }
            if (!columnDao.Insert(new ColumnDTO(0, boardId, -1, "backlog")))
            {
                throw new Exception("cannot add column");
            }
            if (!columnDao.Insert(new ColumnDTO(1, boardId, -1, "in progress")))
            {
                throw new Exception("cannot add column");
            }
            if (!columnDao.Insert(new ColumnDTO(2, boardId, -1, "done")))
            {
                throw new Exception("cannot add column");
            }
            if (!ubd.Insert(new UserBoardDTO(email, boardId, 1+"")))
            {
                throw new Exception("data was not added to the database");
            }
            boardId++;
            return true;
        }

        /// <summary>
        /// Creating a board for the given user with a limit of columns
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="backlog"></param>
        /// <param name="inprogress"></param>
        /// <param name="done"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool CreateBoard(string email, string boardName, int backlog, int inprogress, int done)
        {
            if (boardName == null || boardName.Length < 1) throw new Exception("board name cannot be empty!");
            if (!users.UserExists(email))
            {
                throw new Exception("user " + email + " is not logged in!");
            }
            if (!boards.ContainsKey(email))
            {
                boards.Add(email, new List<Board>());
            }

            if (!users.IsLoggedIn(email)) throw new Exception("user " + email + " is not logged in!");
            foreach (Board board in boards[email])
            {
                if (email == board.GetOwner() && board.getName().Equals(boardName))
                {
                    throw new Exception("a board with such name already exist!");
                }
            }
            boards[email].Add(new Board(boardName, email, boardId, backlog, inprogress, done));
            
            if (!boardd.Insert(new BoardDTO(boardName, email, boardId, backlog, inprogress, done)))
            {
                throw new Exception("Board is not added to the database");
            }
            if (!columnDao.Insert(new ColumnDTO(0, boardId, backlog, "backlog")))
            {
                throw new Exception("cannot add column");
            }
            if (!columnDao.Insert(new ColumnDTO(1, boardId, inprogress, "in progress")))
            {
                throw new Exception("cannot add column");
            }
            if (!columnDao.Insert(new ColumnDTO(2, boardId, done, "done")))
            {
                throw new Exception("cannot add column");
            }
            if (!ubd.Insert(new UserBoardDTO(email, boardId, 1 + "")))
            {
                throw new Exception("data was not added to the database");
            }
            boardId++;
            return true;
        }

        /// <summary>
        /// Deleting a board named boardName for the given user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <exception cref="Exception"></exception>
        public void DeleteBoard(string email, string boardName)
        {
            if (boardName == null || boardName.Length < 1) throw new Exception("board name cannot be empty!");
            if (!users.UserExists(email))
            {
                throw new Exception("user " + email + " is not exists!");
            }

            if (!users.IsLoggedIn(email)) throw new Exception("user " + email + " is not logged in!");
            if (boards.ContainsKey(email))
            {
                LinkedList<Board> boardList = new LinkedList<Board>();
                foreach (Board board in boards[email])
                {
                    if (board.getName().Equals(boardName))
                    {
                        boardList.AddFirst(board);
                    }
                }

                if (boardList.Count == 0)
                {
                    throw new Exception("a board with such name " + boardName +
                                        " doesnt exist in the board of the user!");
                }

                foreach (Board board in boardList)
                {
                    if (board.GetOwner().Equals(email))
                    {
                        if (!taskDao.DeleteBoardTasks(board.GetId()))
                        {
                            throw new Exception("cannot delete the Tasks of the board");
                        }
                        board.DeleteTasks();
                        if (!columnDao.DeleteColumns(board.GetId()))
                        {
                            throw new Exception("cannot delete the columns of the board");
                        }
                        if (!ubd.Delete(board.GetId()))
                        {
                            throw new Exception("delete from user board didn't work");
                        }
                        if (!boardd.Delete(board.GetId()))
                        {
                            throw new Exception("cannot delete the board");
                        }
                        boards[email].Remove(board);
                        return;
                    }
                }

                if (boardList.Count > 0)
                {
                    throw new Exception("The user is not the owner of this board");

                }
            }

            throw new Exception("a user with such name has not anyboard!");

        }

        /// <summary>
        /// Getting number of boards for a user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public int boardsNumber(string email)
        {
            return boards.Count;
        }

        /// <summary>
        /// Adding a task for board named boardName for a given user
        /// </summary>
        /// <param name="title"></param>
        /// <param name="dueDate"></param>
        /// <param name="description"></param>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public bool AddTask(string title, DateTime dueDate, string description, string email, string boardName)
        {
            if (!users.UserExists(email))
            {
                throw new ArgumentException("user doesn't exist");
            }

            if (!boards.ContainsKey(email))
            {
                throw new ArgumentException("Email doesn't have boards");
            }

            if (!users.IsLoggedIn(email))
            {
                throw new ArgumentException("user is not logged in");
            }

            foreach (Board board in boards[email])
            {
                if (board.getName().Equals(boardName))
                {
                    if (!(board.GetOwner().Equals(email)) && !board.emails.Contains(email))
                    {
                        throw new Exception("The user is not member in this board");
                    }

                    if (title.Length == 0) throw new Exception("title cannot be empty!");
                    if (title.Length > 50) throw new Exception("title length cannot be more than 50!");
                    
                    List<Board> boards = GetBoardsOfUser(email);
                    foreach (Board board1 in boards)
                    {
                        if (boardName.Equals(board1.getName()))
                        {
                            if (board1 != null)
                            {
                                if (board1.columns[0].addTask(new Task(board1.getTaskIdCounter(), dueDate, title, description)))
                                {
                                    if (!taskDao.Insert(new TaskDTO(board1.getTaskIdCounter(), DateTime.Now, title, description,
                                            dueDate, 0, board1.GetId(), null)))
                                    {
                                        throw new Exception("cant add the task to the data base");
                                    }
                                }
                                board1.AddOne();
                            }
                            else
                            {
                                throw new Exception("Column doesn't exist");
                            }
                        }
                    }
                    return true;
                }
            }

            throw new Exception("a board with such name doesnt exist!");
            ;
        }

        /// <summary>
        /// Changing the status of a task to another status
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="boardName"></param>
        /// <param name="status"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public bool MoveStatus(int taskId, string boardName, int status, string email)
        {
            if (!users.UserExists(email))
            {
                throw new Exception("user " + email + " is not exists!");
            }

            if (!boards.ContainsKey(email))
            {
                throw new Exception("user " + email + " does not has anyboards");
            }

            if (!users.IsLoggedIn(email))
                throw new Exception("user " + email + " is not logged in!");
            foreach (Board board in boards[email])
            {

                if (board.getName().Equals(boardName))
                {

                    Board b = board.MoveStatus(taskId, status, email);
                    boards[email].Remove(board);
                    boards[email].Add(b);
                    if (!taskDao.Update(taskId, "columnId", (status + 1) + "", b.GetId()))
                    {
                        throw new Exception("update the task in database failed");
                    }
                    return true;
                }
            }

            throw new ArgumentException("There's no board with a name: " + boardName);
        }

        /// <summary>
        /// Getting a column name
        /// </summary>
        /// <param name="status"></param>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public string GetColumnName(int status, string email, string boardName)
        {

            if (!(status == 0 || status == 1 || status == 2))
            {
                throw new ArgumentException("Invalid status");
            }

            if (!users.UserExists(email))
            {
                throw new ArgumentException("User doesn't exist");
            }

            if (!boards.ContainsKey(email))
            {
                throw new ArgumentException("Email doesn't have boards");
            }

            if (!users.IsLoggedIn(email))
                throw new Exception("user " + email + " is not logged in!");
            foreach (Board board in boards[email])
            {
                if (boardName.Equals(board.getName()))
                {
                    return board.getColumn(status).GetName();
                }
            }

            throw new ArgumentException("There's no board with a name: " + boardName);
        }

        /// <summary>
        /// Getting a column
        /// </summary>
        /// <param name="status"></param>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public List<Task> GetColumn(int status, string email, string boardName)
        {
            if (!(status == 0 || status == 1 || status == 2))
            {
                throw new ArgumentException("Invalid status");
            }

            if (!users.UserExists(email))
            {
                throw new ArgumentException("user doesn't exist");
            }

            if (!boards.ContainsKey(email))
            {
                throw new ArgumentException("Email doesn't have boards");
            }

            if (!users.IsLoggedIn(email))
            {
                throw new ArgumentException("user is not logged in");
            }

            foreach (Board board in boards[email])
            {
                if (boardName.Equals(board.getName()))
                {
                    return board.getColumn(status).GetTasks();
                }
            }

            throw new ArgumentException("There's no board with a name: " + boardName);
        }

        /// <summary>
        /// Giving a limit for a columns of a specific board
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public bool LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            if (!users.UserExists(email))
            {
                throw new Exception("user " + email + " is not exists!");
            }

            if (!boards.ContainsKey(email))
            {
                throw new Exception("user " + email + " does not has anyboards");
            }

            if (!users.IsLoggedIn(email))
                throw new Exception("user " + email + " is not logged in!");
            foreach (Board board in boards[email])
            {
                if (board.getName().Equals(boardName))
                {
                    Board b = board.LimitColumn(columnOrdinal, limit);
                    boards[email].Remove(board);
                    boards[email].Add(b);
                    if (!columnDao.Update(columnOrdinal, "maxTask", limit + "", b.GetId()))
                    {
                        throw new Exception("cannot update the column");
                    }
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Getting a limit of a column
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentException"></exception>
        public int GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            if (!(columnOrdinal == 0 || columnOrdinal == 1 || columnOrdinal == 2))
            {
                throw new Exception("Invalid status");
            }

            if (!users.UserExists(email))
            {
                throw new Exception("user " + email + " is not exists!");
            }

            if (!boards.ContainsKey(email))
            {
                throw new Exception("user " + email + " does not has anyboards");
            }

            if (!users.IsLoggedIn(email))
                throw new Exception("user " + email + " is not logged in!");
            foreach (Board board in boards[email])
            {
                if (board.getColumn(columnOrdinal).GetMaxTask() != -1)
                {
                    if (board.getName().Equals(boardName))
                    {
                        return board.GetColumnLimit(columnOrdinal);
                    }
                }
                else if (board.getColumn(columnOrdinal).GetMaxTask() == -1)
                {
                    if (board.getName().Equals(boardName))
                    {
                        return -1;
                    }
                }
            }

            throw new Exception("The user doesn't have a board with a name " + boardName);
        }

        /// <summary>
        /// Getting all boards of a given user, We used this in milestone 1, But we still use it in milestone 2
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public List<Board> GetBoardsOfUser(string email)
        {
            if (!users.UserExists(email))
            {
                throw new Exception("user " + email + " is not exists!");
            }

            if (!boards.ContainsKey(email))
            {
                throw new Exception("user " + email + " does not has anyboards");
            }

            if (!users.IsLoggedIn(email))
                throw new Exception("user " + email + " is not logged in!");
            return boards[email];
        }
        public List<object> GetUserBoards(string email)//new
        {
            if (email == null)
            {
                throw new Exception("email is null");
            }
            List<object> answer = new List<object>();
            if (!users.UserExists(email))
            {
                throw new Exception("user " + email + " is not exists!");
            }
            if (!users.IsLoggedIn(email))
                throw new Exception("user " + email + " is not logged in!");
            List<DTO> boardsofuser = ubd.Select(email);
            foreach (DTO board in boardsofuser)
            {
                UserBoardDTO ubdto = (UserBoardDTO)board;
                answer.Add(ubdto.boardid);
            }
            return answer;
        }
        /// <summary>
        /// Joing the given user to a board with an given id
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public void JoinBoard(string email, int boardID)
        {
            if (!users.UserExists(email))
            {
                throw new Exception("user " + email + " is not exists!");
            }

            if (!users.IsLoggedIn(email))
                throw new Exception("user " + email + " is not logged in!");
            foreach (string user in boards.Keys)
            {
                foreach (Board board in boards[user])
                {
                    if (board.GetId() == boardID)
                    {
                        if (!email.Equals(board.GetOwner()))
                        {
                            if (board.emails.Contains(email))
                            {
                                throw new Exception("This user is already member in this board");
                            }
                            else
                            {
                                board.emails.Add(email);
                                if(!ubd.Insert(new UserBoardDTO(email, boardID, 0 + "")))
                                {
                                    throw new Exception("cannot insert to the database");
                                }
                                return;
                            }
                        }
                        else
                        {
                            throw new Exception("can not added owner to members");
                        }
                    }
                }
            }

            throw new Exception("The given Board Id is not exists");
        }
        /// <summary>
        /// leave a board that user is joined to
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardID"></param>
        /// <exception cref="Exception"></"The owner cannot leave the board">
        public void LeaveBoard(string email, int boardID)
        {
            if (!users.UserExists(email))
            {
                throw new Exception("user " + email + " is not exists!");
            }

            if (!users.IsLoggedIn(email))
                throw new Exception("user " + email + " is not logged in!");
            foreach (string user in boards.Keys)
            {
                foreach (Board board in boards[user])
                {
                    if (board.GetId() == boardID)
                    {
                        if (!email.Equals(board.GetOwner()))
                        {
                            if (board.emails.Contains(email))
                            {
                                reAssigenedTask(email, boardID);
                                if(!taskDao.Update("assignee", "Null", boardID))
                                {
                                    throw new Exception("Removing tasks of the email failed");
                                }
                                board.emails.Remove(email);
                                if (!ubd.Delete(email, boardID))
                                {
                                    throw new Exception("email didn't leave the board");
                                }  
                                return;
                            }
                            else
                            {
                                throw new Exception("The user " + email + " is not a member in this board");
                            }
                        }
                        else
                        {
                            throw new Exception("The owner cannot leave the board");
                        }
                    }
                }
            }

            throw new Exception("The given Board Id is not exists");
        }
        /// <summary>
        /// transfer board Ownership from current owner to another user
        /// </summary>
        /// <param name="currentOwnerEmail"></param>
        /// <param name="newOwnerEmail"></param>
        /// <param name="boardName"></param>
        /// <exception cref="Exception"></exception>
        public void TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            if (boardName == null || boardName.Length < 1) throw new Exception("board name cannot be empty!");
            if (!users.UserExists(currentOwnerEmail))
            {
                throw new Exception("user " + currentOwnerEmail + " is not exists!");
            }

            if (!users.UserExists(newOwnerEmail))
            {
                throw new Exception("user " + newOwnerEmail + " is not exists!");
            }

            if (!users.IsLoggedIn(currentOwnerEmail))
                throw new Exception("user " + currentOwnerEmail + " is not logged in!");
            if (!users.IsLoggedIn(newOwnerEmail)) 
                throw new Exception("user " + newOwnerEmail + " is not logged in!");

            if (boards.ContainsKey(currentOwnerEmail))
            {
                LinkedList<Board> boardList = new LinkedList<Board>();
                foreach (Board board in boards[currentOwnerEmail])
                {
                    Console.WriteLine(board.getName());
                }
                foreach (Board board in boards[currentOwnerEmail])
                {
                    if (board.getName().Equals(boardName))
                    {
                        boardList.AddFirst(board);
                        
                    }
                }
                if (boardList.Count == 0)
                {
                    throw new Exception("a board with such name " + boardName +
                                        " is not exist in the board of the user!");
                }
                foreach (Board board in boardList)
                {
                    if (board.GetOwner().Equals(currentOwnerEmail))
                    {

                        if (board.emails.Contains(newOwnerEmail))
                        {
                            if (boards.ContainsKey(newOwnerEmail))
                            {
                                boards[newOwnerEmail].Add(board);
                            }
                            else
                            {
                                boards.Add(newOwnerEmail, new List<Board>());
                                boards[newOwnerEmail].Add(board);
                            }
                            if (!boardd.Update(board.GetId(), "User", newOwnerEmail, "BoardId"))
                            {
                                throw new Exception("Transferownership didn't work");
                            }
                            if (!ubd.Update("IsOwner", 1+"", newOwnerEmail, board.GetId()))
                            {
                                throw new Exception("Transferownership didn't work");
                            }
                            if (!ubd.Update("IsOwner", 0+"", currentOwnerEmail, board.GetId()))
                            {
                                throw new Exception("Transferownership didn't work");
                            }
                            boards[currentOwnerEmail].Remove(board);
                            board.emails.Remove(newOwnerEmail);
                            board.emails.Add(currentOwnerEmail);
                            board.SetOwner(newOwnerEmail);
                            return;
                        }

                        throw new Exception("The new Owner is not member in the board");
                    }
                }

                if (boardList.Count > 0)
                {
                    throw new Exception("The user is not the owner of this board");

                }
            }

            throw new Exception("a user with such name" + currentOwnerEmail + " has not any board!");
        }


        //help function
        public void reAssigenedTask(string email, int boardId)
        {
            Board b = GetBoard(boardId);
            for (int i = 0; i <= 2; i++)
            {
                foreach (Task task in b.getColumn(i).GetTasks())
                {
                    if (task.assignee == email)
                    {
                        task.assignee = null;
                    }
                }
            }
        }

        public string GetBoardName(int boardId)
        {
            foreach (string user in boards.Keys)
            {
                foreach (Board board in boards[user])
                {
                    if (board.GetId() == boardId)
                    {
                        return board.getName();
                    }
                }
            }

            throw new Exception("There is no board with this id");
        }
        public string LoadData()
        {
            users.LoadData();
            List<DTO> boarDTOs = boardd.Select();
            foreach (DTO board in boarDTOs)
            {
                BoardDTO b = (BoardDTO)board;
                long id = b.GetID();
                if (!boards.ContainsKey(b.GetUser()))
                {
                    List<Board> listb = new List<Board>();
                    List<DTO> bs = boardd.Select(b.GetUser());
                    foreach (DTO bDTO in bs)
                    {
                        BoardDTO bDTO1 = (BoardDTO)bDTO;
                        if (bDTO1.GetUser() == b.GetUser())
                        {
                            List<DTO> columnDTOs = columnDao.Select();
                            int backlog = -1, inprogress = -1, done = -1;
                            foreach (DTO dto in columnDTOs)
                            {
                                ColumnDTO columndto = (ColumnDTO)dto;
                                if (bDTO1.GetID() == columndto.GetBoardId())
                                {
                                    if (columndto.GetId() == 0)
                                    {
                                        if (columndto.GetMaxTask() != -1)
                                        {
                                            backlog = columndto.GetMaxTask();
                                        }
                                    }
                                    if (columndto.GetId() == 1)
                                    {
                                        if (columndto.GetMaxTask() != -1)
                                        {
                                            inprogress = columndto.GetMaxTask();
                                        }
                                    }
                                    if (columndto.GetId() == 2)
                                    {
                                        if (columndto.GetMaxTask() != -1)
                                        {
                                            done = columndto.GetMaxTask();
                                        }
                                    }

                                }
                            }
                            if (backlog == -1 && inprogress == -1 && done == -1)
                                listb.Add(new Board(bDTO1.GetName(), bDTO1.GetUser(), bDTO1.GetID()));
                            else
                                listb.Add(new Board(bDTO1.GetName(), bDTO1.GetUser(), bDTO1.GetID(), backlog, inprogress, done));
                        }
                    }
                    boards.Add(b.GetUser(), listb);
                }
            }

            List<DTO> userboardDTOs = ubd.Select();
            foreach (DTO dto in userboardDTOs)
            {
                UserBoardDTO ubdto = (UserBoardDTO)dto;
                Board board = GetBoard(ubdto.boardid);
                if (!board.emails.Contains(ubdto.email))
                {
                    board.emails.Add(ubdto.email);
                }
            }

            List<DTO> taskDtos = taskDao.Select();
            foreach (DTO task in taskDtos)
            {
                TaskDTO taskDto = (TaskDTO)task;
                foreach (string user in boards.Keys)
                {
                    foreach (Board board in boards[user])
                    {
                        //we should check if the task exists
                        if (board.GetId() == taskDto.GetBoardId())
                        {
                            board.columns[taskDto.GetColumnId()].addTask(new Task(taskDto.GetId(),taskDto.GetDue_time(),taskDto.GetTitle(),taskDto.GetDescription()));
                        }
                    }
                }
            }

            return "";
        }

        public string DeleteData()
        {
            users.DeleteData();
            if (taskDao.Delete())
            {
                foreach(string email in boards.Keys)
                {
                    foreach(Board b in boards[email])
                    {
                        b.columns[0].DeleteTasks();
                        b.columns[1].DeleteTasks();
                        b.columns[2].DeleteTasks();
                    }
                }
            }
            else
            {
                throw new Exception("Database faild to remove Task table");
            }
            if (!ubd.Delete())
            {
                throw new Exception("Database faild to remove userboard table");
            }
            if (!columnDao.Delete())
            {
                throw new Exception("Database faild to remove Column table");
            }
            if (boardd.Delete())
            {
                boards.Clear();
            }
            else
            {
                throw new Exception("Database faild to remove Task table");
            }
            return "";
        }
        public List<Column> GetTasks(int id)
        {
            List<Task> tasks = new List<Task>();
            foreach (string email in boards.Keys)
            {
                foreach (Board b in boards[email])
                {
                    if(b.GetId() == id)
                    {
                        return b.columns;
                    }
                }
            }
            throw new Exception("There's no tasks");
        }
    }
}
