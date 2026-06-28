import { useQuery } from "@tanstack/react-query";
import { coursesService } from "../services/coursesService.ts";

export const useLoadCourse = (courseId: string) => {
  const {
    isPending: isCourseLoading,
    error: courseError,
    data: courseData,
  } = useQuery({
    queryKey: ["course", courseId],
    queryFn: () => coursesService.getCourseById(courseId),
  });

  return { courseError, isCourseLoading, courseData };
};
