import type {
  ICourse,
  ICoursesResponse,
  Lesson,
  Section,
  Subtitle,
} from "../types/ICourse.ts";
import {
  URL_COURSES_API,
  URL_LESSON_API,
  URL_LESSONS_API,
  URL_UPLOAD_API,
} from "../utils/constants.ts";

export const baseUrl = "http://localhost:8080";

export const getCourses = async (): Promise<ICoursesResponse> => {
  const response = await fetch(`${baseUrl}${URL_COURSES_API}`);

  return await response.json();
};

export const getCourseById = async (courseId: string): Promise<ICourse> => {
  const courseUrl = `${baseUrl}${URL_COURSES_API}${courseId}`;

  const response = await fetch(courseUrl);

  return await response.json();
};

export const getCourseSections = async (
  courseId: string,
): Promise<Section[]> => {
  const sectionsUrl = `${baseUrl}${URL_COURSES_API}${courseId}/sections`;

  const response = await fetch(sectionsUrl);
  const data = await response.json();

  return data.sections;
};

export const getLessonsBySectionId = async (
  sectionId: string,
): Promise<Lesson[]> => {
  const lessonsUrl = `${baseUrl}${URL_LESSONS_API}${sectionId}`;

  const response = await fetch(lessonsUrl);
  const data = await response.json();

  return data.lessons;
};

export const getLessonById = async (lessonId: string): Promise<Lesson> => {
  const lessonUrl = `${baseUrl}${URL_LESSON_API}${lessonId}`;

  const response = await fetch(lessonUrl);

  return await response.json();
};

export const getLessonSubtitles = async (
  lessonId: string,
): Promise<Subtitle[]> => {
  const lessonsUrl = `${baseUrl}${URL_LESSONS_API}${lessonId}/subtitles`;

  const response = await fetch(lessonsUrl);
  const data = await response.json();

  return data.subtitles;
};

export const uploadCourse = async (
  path: string,
  isSingleCourse: boolean,
): Promise<boolean> => {
  const uploadUrl = `${baseUrl}${URL_UPLOAD_API}${isSingleCourse ? "single" : "bunch"}`;

  const formData = new FormData();

  formData.append("rootPath", path);

  const response = await fetch(uploadUrl, {
    method: "POST",
    body: formData,
  });

  const status = response.status;

  return status === 201;
};

export const deleteCourse = async (courseId: string): Promise<boolean> => {
  const deleteUrl = `${baseUrl}${URL_COURSES_API}${courseId}`;

  const response = await fetch(deleteUrl, {
    method: "DELETE",
  });

  const status = response.status;

  return status === 204;
};

export const updateLessonCompletion = async (
  lessonId: string,
  completion: boolean,
): Promise<boolean> => {
  const updateUrl = `${baseUrl}${URL_LESSONS_API}${lessonId}?isCompleted=${completion}`;

  const response = await fetch(updateUrl, {
    method: "PATCH",
  });

  const status = response.status;

  return status === 204;
};

export const getLessonTextContent = async (
  lessonId: string,
): Promise<string> => {
  const textContentUrl = `${baseUrl}${URL_LESSONS_API}${lessonId}/text`;

  const response = await fetch(textContentUrl);
  const text = await response.text();

  return text;
};
