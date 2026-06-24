export interface ICoursesResponse {
  data: ICourse[];
  meta: IBasicMeta;
  links: {
    self: string;
    next: string | null;
    previous: string | null;
  };
}

export interface IPostCourseRequest {
  path: string;
  isSingleCourse: boolean;
}

interface IBasicMeta {
  totalItems: number;
  totalPages: number;
  currentPage: number;
  pageSize: number;
  offset: number;
}

export interface ICourse {
  courseId: string;
  courseName: string;
}

export interface Section {
  sectionId: string;
  sectionName: string;
}

export interface Lesson {
  lessonId: string;
  sectionId: string;
  lessonName: string;
  lessonType: "video" | "text";
  streamPath: string;
  isCompleted: boolean;
  lessonDurationSeconds?: number;
}

export interface Subtitle {
  subtitleId: string;
  subtitleName: string;
  languageCode: string;
  subtitleType: "str" | "vtt" | "unknown";
  rootPath: string;
}
