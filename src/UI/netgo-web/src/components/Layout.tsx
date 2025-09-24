import React from "react";

interface LayoutProps {
  title?: string;
  children: React.ReactNode;
}

const Layout: React.FC<LayoutProps> = ({ children }) => {
  return (
    <main className="min-h-screen">
      <div className="max-w-6xl mx-auto p-4">{children}</div>
    </main>
  );
};

export default Layout;
