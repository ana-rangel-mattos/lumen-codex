import { ErrorComponent } from "@tanstack/react-router";
import { CourseCard, Spinner } from "../../components";
import { useLoadCourses } from "../../hooks/useLoadCourses.ts";

const Dashboard = () => {
  const { isPending, error, data } = useLoadCourses();

  if (isPending) {
    return <Spinner />;
  }

  if (error) {
    return <ErrorComponent error={error} />;
  }

  return (
    <div className="mt-6 px-8 pb-6">
      <h2 className="text-base-100 text-4xl">
        Courses Dashboard{" "}
        <span className="text-base-300 text-xl">
          - Total courses {data?.meta.totalItems}
        </span>
      </h2>

      <ul className="mt-8 grid grid-cols-[repeat(auto-fill,minmax(24rem,1fr))] items-end gap-6">
        {data?.data?.map((course) => (
          <CourseCard key={course.courseId} course={course} />
        ))}
      </ul>

      <div className="join">
        {data && data.meta.currentPage > 1 && (
          <button className="join-item btn">«</button>
        )}
        {data && (
          <button className="join-item btn">
            Page {data.meta.currentPage}
          </button>
        )}
        {data && data.meta.totalPages > data.meta.currentPage && (
          <button className="join-item btn">»</button>
        )}
      </div>
    </div>
  );
};

export default Dashboard;
