<template>
  <div class="min-h-screen text-neutral-100">
    <header class="border-b border-neutral-800 bg-black/80">
      <div class="mx-auto flex max-w-7xl flex-col items-start gap-3 px-4 py-4 sm:flex-row sm:items-center sm:justify-between sm:px-5">
        <RouterLink to="/albumes" class="text-base font-bold uppercase tracking-[0.18em] text-lime-200">
          ./albumranker
        </RouterLink>

        <nav class="flex w-full items-center gap-2 sm:w-auto">
          <RouterLink to="/albumes" class="terminal-link">
            ./albumes
          </RouterLink>

          <span class="text-neutral-700">|</span>

          <RouterLink to="/comparativas" class="terminal-link">
            ./comparar
          </RouterLink>
        </nav>
      </div>
    </header>

    <main class="mx-auto max-w-7xl px-5 py-6">
      <RouterView />
    </main>
  </div>
</template>
