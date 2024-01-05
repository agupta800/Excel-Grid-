using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace AsterWebApp.Models
{
    [DataContract]
    public class ChunkMetaData
    {
        [Key]
        public int Id { get; set; }

        [DataMember(Name = "uploadUid")]
        public string UploadUid { get; set; } = String.Empty;
        [DataMember(Name = "fileName")]
        public string FileName { get; set; } = String.Empty;
        [DataMember(Name = "contentType")]
        public string ContentType { get; set; } = String.Empty;
        [DataMember(Name = "chunkIndex")]
        public long ChunkIndex { get; set; }
        [DataMember(Name = "totalChunks")]
        public long TotalChunks { get; set; }
        [DataMember(Name = "totalFileSize")]
        public long TotalFileSize { get; set; }
    }
    public class FileResult
    {
        [Key]
        public int Id { get; set; }
        public bool uploaded { get; set; }
        public string fileUid { get; set; } = String.Empty;
    }

    public class FileUploadRequest
    {
        [Key]
        public int Id { get; set; }
        public string Entity { get; set; }
        public string EntityId { get; set; }
        public string FileTitle { get; set; }
    }

    public class DropDownModel
    {
        public long Id { get; set; }

      
        public SelectList List { get; set; }
    }

    public class MediaHelper
    {
        

        public const string ROOT = "App_Data";
        public const string SCHOOL = "/School";
        public const string SCHOOL_ID = "/School/{SCHOOL_ID}";
        public const string SCHOOL_PRIVATE = "/School/{SCHOOL_ID}/Private";
        public const string SCHOOL_PUBLIC = "/School/{SCHOOL_ID}/Public";

        public const string IEMIS_FILE = "IEMIS";

        //public const string MEDIA_FOLDERS = "/School/{SCHOOL_ID}/MediaFolders";
        //public const string MEDIA_FOLDERS_ID = "/School/{SCHOOL_ID}/MediaFolders/{MEDIA_FOLDER_ID}";

        public const string BOOKS = "/School/{SCHOOL_ID}/Private/Books/{BOOK_ID}";
        public const string STUDENTS = "/School/{SCHOOL_ID}/Private/Students/{STUDENT_ID}";

        public const string SUBJECTS_PUBLIC = "/School/{SCHOOL_ID}/Public/Subjects/{SUBJECT_CODE}";
        public const string COURSES_PUBLIC = "/School/{SCHOOL_ID}/Public/Courses/{COURSE_ID}";

        public const string CLASSES = "/School/{SCHOOL_ID}/Private/Classes/{CLASS_NUMBER}";
        public const string CLASS_SUBJECTS = "/School/{SCHOOL_ID}/Private/Classes/{CLASS_NUMBER}/Subjects/{SUBJECT_CODE}";
        public const string SUBJECT_COURSES = "/School/{SCHOOL_ID}/Private/Classes/{CLASS_NUMBER}/Subjects/{SUBJECT_CODE}/Courses/{COURSE_ID}";
        public const string SCHOOL_COURSE_QUESTIONS = "/School/{SCHOOL_ID}/Private/Questions/{CLASS_NUMBER}/Subjects/{SUBJECT_CODE}";


        public const string TEACHERS = "/School/{SCHOOL_ID}/Private/Teachers/{TEACHER_ID}";
        public const string PARENTS = "/School/{SCHOOL_ID}/Private/Parents/{PARENT_ID}";
        public const string STAFFS = "/School/{SCHOOL_ID}/Private/Staffs/{STAFF_ID}";

        public static string GetRoot()
        {
            return $@"\" + ROOT;
        }

        public static string GetSchoolPath(string SCHOOL_ID)
        {
            return $@"\App_Data\School\{SCHOOL_ID}";
        }

        public static string GetSchoolPrivateFolderPath(string SCHOOL_ID)
        {
            return $@"\App_Data\School\{SCHOOL_ID}\Private";
        }

        public static string GetSchoolPublicFolderPath(string SCHOOL_ID)
        {
            return $@"\App_Data\School\{SCHOOL_ID}\Public";
        }

        public static string GetMediaFolderIdPath(string SCHOOL_ID, string MEDIA_FOLDER_ID)
        {
            return $@"\App_Data\School\{SCHOOL_ID}\Private\{MEDIA_FOLDER_ID}";
        }

        public static string GetEntityPath(string SCHOOL_ID, string MEDIA_FOLDER_ID, string ENTITY, string id)
        {
            return $@"\App_Data\School\{SCHOOL_ID}\Private\{MEDIA_FOLDER_ID}\{ENTITY}\{id}";
        }
        public static string GetBooksPath(string SCHOOL_ID, string MEDIA_FOLDER_ID)
        {
            return $@"\App_Data\School\{SCHOOL_ID}\Private\{MEDIA_FOLDER_ID}\Books";
        }

        public static string GetCoursePath(string SCHOOL_ID, string MEDIA_FOLDER_ID)
        {
            return $@"\App_Data\School\{SCHOOL_ID}\Private\{MEDIA_FOLDER_ID}\Courses";
        }

        public static string GetStudentPath(string SCHOOL_ID, string MEDIA_FOLDER_ID)
        {
            return $@"\App_Data\School\{SCHOOL_ID}\Private\{MEDIA_FOLDER_ID}\Students";
        }
        public static string GetTeacherPath(string SCHOOL_ID, string MEDIA_FOLDER_ID)
        {
            return $@"\App_Data\School\{SCHOOL_ID}\Private\{MEDIA_FOLDER_ID}\Teachers";
        }
        public static string GetParentPath(string SCHOOL_ID, string MEDIA_FOLDER_ID)
        {
            return $@"\App_Data\School\{SCHOOL_ID}\Private\{MEDIA_FOLDER_ID}\Parents";
        }
        public static string GetQuestionPath(string PROVINCE_ID, string DISTRICT_CODE, string SCHOOL_ID)
        {
            return $@"\App_Data\School\{PROVINCE_ID}\{DISTRICT_CODE}\{SCHOOL_ID}\Private\Questions";
        }
    }
}
