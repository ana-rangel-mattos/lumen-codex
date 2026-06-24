import { ApiClient } from "./apiClient.ts";
import { BASE_URL, URL_COURSES_API } from "../utils/constants.ts";
import type {
  ICourse,
  ICoursesResponse,
  IPostCourseRequest,
} from "../types/ICourse.ts";

class CoursesService extends ApiClient {
  constructor() {
    super(`${BASE_URL}${URL_COURSES_API}`);
  }

  getCourses(): Promise<ICoursesResponse> {
    return this.get("");
  }

  getCourseById(id: string): Promise<ICourse> {
    return this.get(id);
  }

  uploadCourse(request: IPostCourseRequest): Promise<void> {
    return this.post(`upload/${request.isSingleCourse ? "single" : "bunch"}`, { RootPath: request.path })
  }

  deleteCourse(id: string): Promise<void> {
    return this.delete(id);
  }
}

export const coursesService = new CoursesService();