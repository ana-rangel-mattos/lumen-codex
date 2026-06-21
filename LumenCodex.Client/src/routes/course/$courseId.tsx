import { createFileRoute } from "@tanstack/react-router";
import { CoursePage } from "../../pages";

export const Route = createFileRoute("/course/$courseId")({
  component: CoursePage,
});
