import type { Section } from "../../types/course.ts";
import * as React from "react";
import { useQuery } from "@tanstack/react-query";
import { getLessonsBySectionId } from "../../services/courseApi.ts";
import { Spinner } from "../index.ts";
import { ErrorComponent, Link } from "@tanstack/react-router";
import { secondsToTime } from "../../utils/timeConvertion.ts";

interface ICourseSectionProps {
  section: Section;
  courseId: string;
}

const CourseSection: React.FC<ICourseSectionProps> = ({
  section,
  courseId,
}) => {
  const { isPending, error, data } = useQuery({
    queryKey: ["lesson", section.sectionId],
    queryFn: () => getLessonsBySectionId(section.sectionId),
  });

  if (isPending) {
    return <Spinner />;
  }

  if (error) {
    return <ErrorComponent error={error} />;
  }

  return (
    <div className="custom-scrollbar flex-1">
      <div className="collapse-arrow join-item border-secondary-content border-b1 collapse rounded-none">
        <input type="checkbox" />
        <div className="collapse-title bg-base-content text-primary-content font-medium">
          {section.sectionName}
        </div>

        <div className="collapse-content p-0">
          <ul className="menu w-full overflow-auto p-0">
            {data?.map((lesson) => (
              <li
                key={lesson.lessonId}
                className="bg-secondary text-base-100 border-accent/50 border-b-2 last:border-none"
              >
                <Link
                  params={{
                    lessonId: lesson.lessonId,
                    courseId,
                  }}
                  search={{
                    nextLessonId: data[data?.indexOf(lesson) + 1]?.lessonId,
                    prevLessonId: data[data?.indexOf(lesson) - 1]?.lessonId,
                  }}
                  to="/course/$courseId/$lessonId"
                  className="hover:bg-accent/70 flex items-start gap-3 rounded-none py-4"
                >
                  <input
                    type="checkbox"
                    checked={lesson.isCompleted}
                    className="checkbox checkbox-success checkbox-md mt-2"
                    readOnly
                  />
                  <div className="flex flex-col items-start">
                    <span className="text-sm leading-tight font-medium">
                      {lesson.lessonName}
                    </span>
                    <span className="mt-1 flex items-center gap-1 text-xs opacity-50">
                      <i className="hn hn-clock-solid"></i>
                      {secondsToTime(lesson.lessonDurationSeconds) || "No data"}
                    </span>
                  </div>
                </Link>
              </li>
            ))}
          </ul>
        </div>
      </div>
    </div>
  );
};

export default CourseSection;
