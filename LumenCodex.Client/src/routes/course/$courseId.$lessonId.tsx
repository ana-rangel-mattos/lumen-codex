import { createFileRoute } from "@tanstack/react-router";
import { LessonPage } from "../../pages";

type LessonSearch = {
  nextLessonId?: string;
  prevLessonId?: string;
};

export const Route = createFileRoute("/course/$courseId/$lessonId")({
  component: LessonPage,
  validateSearch: (search: Record<string, unknown>): LessonSearch => {
    return {
      nextLessonId: (search.nextLessonId as string) || undefined,
      prevLessonId: (search.prevLessonId as string) || undefined,
    };
  },
});
