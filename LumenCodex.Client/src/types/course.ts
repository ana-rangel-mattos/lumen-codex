export interface ICoursesResponse {
  data: Course[];
  meta: IBasicMeta;
  links: {
    self: string;
    next: string | null;
    previous: string | null;
  };
}

interface IBasicMeta {
  totalItems: number;
  totalPages: number;
  currentPage: number;
  pageSize: number;
  offset: number;
}

export interface Course {
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
