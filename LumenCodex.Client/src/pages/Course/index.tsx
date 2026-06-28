import {
  ErrorComponent,
  Link,
  Outlet,
  useParams,
} from "@tanstack/react-router";
import { CourseSection, Spinner } from "../../components";
import { useState } from "react";
import { useLoadCourse } from "../../hooks/useLoadCourse.ts";
import { useCourseSections } from "../../hooks/useCourseSections.ts";

const CoursePage = () => {
  const { courseId } = useParams({ from: "/course/$courseId" });
  const [isSidebarOpen, setIsSidebarOpen] = useState<boolean>(true);

  const { isSectionLoading, sectionError, sectionsData } =
    useCourseSections(courseId);
  const { isCourseLoading, courseError, courseData } = useLoadCourse(courseId);

  if (isSectionLoading || isCourseLoading) {
    return <Spinner />;
  }

  if (sectionError || courseError) {
    return <ErrorComponent error={sectionError || courseError} />;
  }

  return (
    <div className="flex h-full flex-col">
      {/* Course Header */}
      <div className="flex">
        <div className="navbar bg-base-200 flex justify-between px-4 sm:flex-col">
          <div className="breadcrumbs text-base-100 text-md">
            <ul>
              <li>
                <Link to="/dashboard">
                  <i className="hn hn-home-solid"></i>
                </Link>
              </li>
              <li>
                {courseData?.courseId && (
                  <Link
                    params={{ courseId: courseData.courseId }}
                    to="/course/$courseId"
                  >
                    {courseData.courseName}
                  </Link>
                )}
              </li>
              <li>This lesson</li>
            </ul>
          </div>
          <h1 className="text-base-100 truncate text-2xl sm:text-base">
            {courseData?.courseName}
          </h1>
        </div>
      </div>

      <div className="h-full lg:grid lg:grid-cols-[1fr_1fr_1fr_1fr_1fr_1fr] lg:grid-rows-[1fr_1fr_1fr_1fr]">
        {/*Course Video Player*/}
        <Outlet />

        {/*Course Sidebar*/}
        <div
          className={`bg-secondary/20 border-accent overflow-y-auto border-l-2 sm:w-full ${isSidebarOpen ? "lg:relative lg:col-start-1 lg:col-end-2 lg:row-start-1 lg:row-end-5 lg:w-120" : "lg:col-start-1 lg:col-end-5 lg:row-start-3 lg:row-end-5 lg:w-full"}`}
        >
          <button
            onClick={() => setIsSidebarOpen((prev) => !prev)}
            className={`absolute top-4 right-4 text-white hover:text-gray-300 sm:hidden`}
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="24"
              height="24"
              viewBox="0 0 24 24"
              fill="currentColor"
              className="icon icon-tabler icons-tabler-filled icon-tabler-layout-sidebar-left-collapse"
            >
              <path stroke="none" d="M0 0h24v24H0z" fill="none" />
              <path d="M18 3a3 3 0 0 1 2.995 2.824l.005 .176v12a3 3 0 0 1 -2.824 2.995l-.176 .005h-12a3 3 0 0 1 -2.995 -2.824l-.005 -.176v-12a3 3 0 0 1 2.824 -2.995l.176 -.005h12zm0 2h-9v14h9a1 1 0 0 0 .993 -.883l.007 -.117v-12a1 1 0 0 0 -.883 -.993l-.117 -.007zm-2.293 4.293a1 1 0 0 1 .083 1.32l-.083 .094l-1.292 1.293l1.292 1.293a1 1 0 0 1 .083 1.32l-.083 .094a1 1 0 0 1 -1.32 .083l-.094 -.083l-2 -2a1 1 0 0 1 -.083 -1.32l.083 -.094l2 -2a1 1 0 0 1 1.414 0z" />
            </svg>
          </button>

          <div className="border-base-300 border-b p-4">
            <h2 className="text-base-100 text-xl">Course Content</h2>
            <progress
              className="progress progress-secondary w-full"
              value="40"
              max="100"
            ></progress>
          </div>

          {sectionsData?.map((section) => (
            <CourseSection
              key={section.sectionId}
              section={section}
              courseId={courseId}
            />
          ))}
        </div>
      </div>
    </div>
  );
};

export default CoursePage;
