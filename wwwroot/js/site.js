// Halil Mert Develi — portfolio v2 interactions.
// Small, dependency-free, and every effect degrades to a static page.
(() => {
  "use strict";

  const root = document.documentElement;
  const reduceMotion = window.matchMedia("(prefers-reduced-motion: reduce)");
  const finePointer = window.matchMedia("(hover: hover) and (pointer: fine)");

  /* ---------- Navigation ---------- */
  const nav = document.querySelector("[data-nav]");
  const toggle = document.querySelector("[data-nav-toggle]");
  const menu = document.querySelector("[data-nav-menu]");

  if (nav) {
    const onScroll = () => nav.classList.toggle("is-scrolled", window.scrollY > 12);
    onScroll();
    window.addEventListener("scroll", onScroll, { passive: true });
  }

  if (nav && toggle && menu) {
    const label = toggle.querySelector(".sr-only");
    const setOpen = (open) => {
      nav.classList.toggle("is-open", open);
      toggle.setAttribute("aria-expanded", String(open));
      document.body.style.overflow = open ? "hidden" : "";
      if (label) label.textContent = open ? label.dataset.labelClose : label.dataset.labelOpen;
    };

    toggle.addEventListener("click", () => setOpen(!nav.classList.contains("is-open")));
    menu.querySelectorAll("a").forEach((link) => link.addEventListener("click", () => setOpen(false)));
    document.addEventListener("keydown", (e) => {
      if (e.key === "Escape" && nav.classList.contains("is-open")) {
        setOpen(false);
        toggle.focus();
      }
    });
    window.matchMedia("(min-width: 961px)").addEventListener("change", (e) => {
      if (e.matches) setOpen(false);
    });
  }

  /* ---------- Current section in nav ---------- */
  const navLinks = [...document.querySelectorAll("[data-nav-link]")];
  const sections = navLinks
    .map((link) => document.querySelector(link.getAttribute("href")))
    .filter(Boolean);

  if ("IntersectionObserver" in window && sections.length) {
    const spy = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          if (!entry.isIntersecting) return;
          navLinks.forEach((link) => {
            const active = link.getAttribute("href") === `#${entry.target.id}`;
            link.classList.toggle("is-current", active);
            if (active) link.setAttribute("aria-current", "location");
            else link.removeAttribute("aria-current");
          });
        });
      },
      { rootMargin: "-45% 0px -50% 0px" }
    );
    sections.forEach((s) => spy.observe(s));
  }

  /* ---------- Reveal on scroll ---------- */
  const revealables = document.querySelectorAll("main [data-reveal]:not(.hero [data-reveal])");
  if ("IntersectionObserver" in window && !reduceMotion.matches) {
    const io = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          if (!entry.isIntersecting) return;
          entry.target.classList.add("is-visible");
          entry.target.querySelectorAll(".viz").forEach((v) => v.classList.add("is-visible"));
          io.unobserve(entry.target);
        });
      },
      { rootMargin: "0px 0px -8% 0px", threshold: 0.08 }
    );

    // Stagger siblings in grids so cards arrive in sequence.
    revealables.forEach((el) => {
      const siblings = el.parentElement ? [...el.parentElement.children].filter((c) => c.hasAttribute("data-reveal")) : [];
      const index = siblings.indexOf(el);
      if (index > 0) el.style.setProperty("--delay", `${Math.min(index, 5) * 70}ms`);
      io.observe(el);
    });
  } else {
    revealables.forEach((el) => el.classList.add("is-visible"));
    document.querySelectorAll(".viz").forEach((v) => v.classList.add("is-visible"));
  }

  /* ---------- Subtle depth on cards (fine pointers only) ---------- */
  if (finePointer.matches && !reduceMotion.matches) {
    document.querySelectorAll("[data-tilt]").forEach((el) => {
      const max = el.classList.contains("project") ? 2.5 : 3.5;
      let frame = 0;
      el.addEventListener("pointermove", (e) => {
        cancelAnimationFrame(frame);
        frame = requestAnimationFrame(() => {
          const r = el.getBoundingClientRect();
          const x = (e.clientX - r.left) / r.width - 0.5;
          const y = (e.clientY - r.top) / r.height - 0.5;
          el.style.setProperty("--ry", `${(x * max).toFixed(2)}deg`);
          el.style.setProperty("--rx", `${(-y * max).toFixed(2)}deg`);
        });
      });
      el.addEventListener("pointerleave", () => {
        cancelAnimationFrame(frame);
        el.style.setProperty("--ry", "0deg");
        el.style.setProperty("--rx", "0deg");
      });
    });
  }

  const animate = !reduceMotion.matches;
  const onceVisible = (el, fn, options = { threshold: 0.3 }) => {
    if (!("IntersectionObserver" in window)) return fn(el);
    const io = new IntersectionObserver(([entry]) => {
      if (!entry.isIntersecting) return;
      io.disconnect();
      fn(el);
    }, options);
    io.observe(el);
  };

  /* ---------- Scroll progress ---------- */
  const progress = document.querySelector("[data-progress]");
  if (progress) {
    let queued = false;
    const paint = () => {
      queued = false;
      const max = root.scrollHeight - window.innerHeight;
      progress.style.transform = `scaleX(${max > 0 ? Math.min(window.scrollY / max, 1).toFixed(4) : 0})`;
    };
    const queue = () => {
      if (queued) return;
      queued = true;
      requestAnimationFrame(paint);
    };
    paint();
    window.addEventListener("scroll", queue, { passive: true });
    window.addEventListener("resize", queue, { passive: true });
  }

  /* ---------- Cursor light: lights up the LED dots under the pointer ---------- */
  if (finePointer.matches && animate) {
    document.querySelectorAll("[data-spot]").forEach((el) => {
      el.addEventListener("pointermove", (e) => {
        const r = el.getBoundingClientRect();
        el.style.setProperty("--mx", `${(e.clientX - r.left).toFixed(0)}px`);
        el.style.setProperty("--my", `${(e.clientY - r.top).toFixed(0)}px`);
      }, { passive: true });
    });
  }

  /* ---------- Counters ---------- */
  // The final number is already in the markup, so no-JS and reduced motion show the real value.
  document.querySelectorAll("[data-count]").forEach((el) => {
    if (!animate) return;
    const target = Number(el.dataset.count);
    const format = new Intl.NumberFormat(root.lang || "tr");
    onceVisible(el, () => {
      const started = performance.now();
      const duration = 1400;
      const tick = (now) => {
        const p = Math.min((now - started) / duration, 1);
        el.textContent = format.format(Math.round(target * (1 - Math.pow(1 - p, 4))));
        if (p < 1) requestAnimationFrame(tick);
      };
      requestAnimationFrame(tick);
    });
  });

  /* ---------- Eyebrows decode like a controller boot line ---------- */
  if (animate) {
    const glyphs = "01#/<>_+*";
    document.querySelectorAll(".eyebrow").forEach((el) => {
      const node = [...el.childNodes].reverse().find((n) => n.nodeType === Node.TEXT_NODE && n.textContent.trim());
      if (!node) return;
      const text = node.textContent;
      onceVisible(el, () => {
        const started = performance.now();
        const tick = (now) => {
          const shown = Math.floor((now - started) / 28);
          if (shown >= text.length) {
            node.textContent = text;
            return;
          }
          node.textContent = text.slice(0, shown) + [...text.slice(shown)].map((c) => (c === " " ? " " : glyphs[Math.floor(Math.random() * glyphs.length)])).join("");
          requestAnimationFrame(tick);
        };
        requestAnimationFrame(tick);
      }, { threshold: 0.6 });
    });
  }

  /* ---------- Terminal types its real output ---------- */
  const terminal = document.querySelector("[data-terminal]");
  if (terminal && animate) {
    const lines = [...terminal.querySelectorAll("[data-line]")];
    const full = lines.map((line) => line.textContent);
    lines.forEach((line) => {
      line.textContent = "";
      line.classList.add("is-pending");
    });
    onceVisible(terminal, () => {
      let index = 0;
      const next = () => {
        if (index >= lines.length) {
          terminal.classList.add("is-done");
          return;
        }
        const line = lines[index];
        const text = full[index];
        line.classList.remove("is-pending");
        line.classList.add("is-typing");
        const typed = line.hasAttribute("data-typed");
        let shown = typed ? 0 : text.length;
        const step = () => {
          line.textContent = text.slice(0, shown);
          if (shown < text.length) {
            shown += 1;
            setTimeout(step, 18);
            return;
          }
          line.classList.remove("is-typing");
          index += 1;
          setTimeout(next, typed ? 240 : 110);
        };
        step();
      };
      next();
    });
  }

  /* ---------- Project filter ---------- */
  const filterBar = document.querySelector("[data-filters]");
  if (filterBar) {
    const buttons = [...filterBar.querySelectorAll("[data-filter]")];
    const items = [...document.querySelectorAll("[data-tags]")];
    const count = document.querySelector("[data-filter-count]");
    const apply = (filter) => {
      let visible = 0;
      items.forEach((item) => {
        const show = filter === "all" || item.dataset.tags.split(" ").includes(filter);
        item.hidden = !show;
        if (show) visible += 1;
        // Reveal immediately: a card that was filtered out never crossed the observer.
        item.classList.add("is-visible");
      });
      buttons.forEach((b) => b.setAttribute("aria-pressed", String(b.dataset.filter === filter)));
      if (count) count.textContent = String(visible);
    };
    buttons.forEach((button) => {
      button.addEventListener("click", () => {
        const run = () => apply(button.dataset.filter);
        if (animate && !document.hidden && document.startViewTransition) {
          // A skipped transition rejects these promises; the filter has still been applied.
          const transition = document.startViewTransition(run);
          transition.ready.catch(() => {});
          transition.finished.catch(() => {});
        } else {
          run();
        }
      });
    });
  }

  /* ---------- LED ticker ---------- */
  // Rasterises the text once, then scrolls it across a dot grid like a real LED sign.
  const ticker = document.querySelector("[data-ticker]");
  if (ticker && ticker.getContext) {
    const ctx = ticker.getContext("2d", { alpha: true });
    const rows = 15;
    const palette = [[243, 181, 98], [185, 163, 255], [141, 182, 255], [95, 211, 160]];
    let lit = new Uint8Array(0), textCols = 1, gap = 5, cols = 0, w = 0, dpr = 1;
    let running = false, visible = true, raf = 0, offset = 0, last = 0;

    const rasterise = () => {
      const source = document.createElement("canvas");
      const sctx = source.getContext("2d", { willReadFrequently: true });
      const font = `700 ${rows - 2}px ${getComputedStyle(root).getPropertyValue("--font-sans")}`;
      sctx.font = font;
      const text = ticker.dataset.ticker;
      textCols = Math.ceil(sctx.measureText(text).width) + 40;
      source.width = textCols;
      source.height = rows;
      sctx.font = font;
      sctx.textBaseline = "middle";
      sctx.fillStyle = "#fff";
      sctx.fillText(text, 0, rows / 2 + 1);
      const data = sctx.getImageData(0, 0, textCols, rows).data;
      lit = new Uint8Array(textCols * rows);
      for (let i = 0; i < lit.length; i++) lit[i] = data[i * 4 + 3] > 96 ? 1 : 0;
    };

    const resize = () => {
      gap = window.innerWidth < 640 ? 4 : 5;
      dpr = Math.min(window.devicePixelRatio || 1, 2);
      w = ticker.getBoundingClientRect().width;
      const h = rows * gap;
      ticker.style.height = `${h}px`;
      ticker.width = Math.round(w * dpr);
      ticker.height = Math.round(h * dpr);
      ctx.setTransform(dpr, 0, 0, dpr, 0, 0);
      cols = Math.ceil(w / gap) + 1;
      draw();
    };

    const draw = () => {
      ctx.clearRect(0, 0, w, rows * gap);
      const shift = Math.floor(offset);
      const size = gap - 1.5;
      const span = Math.max(cols, 1);
      for (let x = 0; x < cols; x++) {
        const col = (x + shift) % textCols;
        const band = ((col / textCols) * palette.length * 3) % palette.length;
        const [r, g, b] = palette[Math.floor(band)];
        const edge = Math.min(x, span - x) / 14;
        const fade = Math.min(edge, 1);
        for (let y = 0; y < rows; y++) {
          const on = lit[y * textCols + col];
          ctx.fillStyle = on ? `rgba(${r},${g},${b},${(0.95 * fade).toFixed(3)})` : "rgba(255,255,255,0.045)";
          ctx.fillRect(x * gap, y * gap, size, size);
        }
      }
    };

    const loop = (now) => {
      offset = (offset + Math.min(now - last, 50) * 0.022) % textCols;
      last = now;
      draw();
      raf = requestAnimationFrame(loop);
    };

    const play = () => {
      if (running || reduceMotion.matches || !visible || document.hidden) return;
      running = true;
      last = performance.now();
      raf = requestAnimationFrame(loop);
    };

    const pause = () => {
      running = false;
      cancelAnimationFrame(raf);
    };

    const boot = () => {
      rasterise();
      resize();
      play();
    };

    // The webfont changes the letter shapes, so rasterise after it has loaded.
    (document.fonts && document.fonts.ready ? document.fonts.ready : Promise.resolve()).then(boot);
    window.addEventListener("resize", () => {
      clearTimeout(resize.t);
      resize.t = setTimeout(resize, 120);
    }, { passive: true });
    if ("IntersectionObserver" in window) {
      new IntersectionObserver(([entry]) => {
        visible = entry.isIntersecting;
        visible ? play() : pause();
      }).observe(ticker);
    }
    document.addEventListener("visibilitychange", () => (document.hidden ? pause() : play()));
  }

  /* ---------- Copy email ---------- */
  const status = document.querySelector("[data-copy-status]");
  document.querySelectorAll("[data-copy]").forEach((btn) => {
    btn.addEventListener("click", async () => {
      const value = btn.getAttribute("data-copy");
      const label = btn.querySelector("[data-copy-label]");
      const original = label ? label.textContent : "";
      try {
        await navigator.clipboard.writeText(value);
        if (label) label.textContent = btn.dataset.copiedLabel;
        if (status) status.textContent = btn.dataset.copiedLabel;
        setTimeout(() => {
          if (label) label.textContent = original;
        }, 2000);
      } catch {
        window.location.href = `mailto:${value}`;
      }
    });
  });

  /* ---------- LED matrix background ---------- */
  // A dot grid that reads like an LED panel. Dots near the pointer light up,
  // and a slow diagonal wave passes through. Paused off-screen and when hidden.
  const canvas = document.querySelector("[data-matrix]");
  if (canvas && canvas.getContext) {
    const ctx = canvas.getContext("2d", { alpha: true });
    const hero = canvas.parentElement;
    const gap = 22;
    let w = 0, h = 0, dpr = 1, cols = 0, rows = 0;
    let pointer = { x: -9999, y: -9999, tx: -9999, ty: -9999 };
    let running = false, visible = true, raf = 0, start = performance.now();

    const resize = () => {
      const rect = hero.getBoundingClientRect();
      dpr = Math.min(window.devicePixelRatio || 1, 2);
      w = rect.width;
      h = rect.height;
      canvas.width = Math.round(w * dpr);
      canvas.height = Math.round(h * dpr);
      ctx.setTransform(dpr, 0, 0, dpr, 0, 0);
      cols = Math.ceil(w / gap) + 1;
      rows = Math.ceil(h / gap) + 1;
      draw(performance.now());
    };

    const draw = (now) => {
      const t = (now - start) / 1000;
      ctx.clearRect(0, 0, w, h);
      pointer.x += (pointer.tx - pointer.x) * 0.12;
      pointer.y += (pointer.ty - pointer.y) * 0.12;
      const animate = !reduceMotion.matches;

      for (let i = 0; i < cols; i++) {
        const x = i * gap + 4;
        for (let j = 0; j < rows; j++) {
          const y = j * gap + 4;
          let glow = 0;
          if (animate) {
            const wave = Math.sin((x * 0.6 + y) * 0.012 - t * 0.9);
            glow = Math.max(0, wave - 0.82) * 1.6;
            const dx = x - pointer.x;
            const dy = y - pointer.y;
            const d2 = dx * dx + dy * dy;
            if (d2 < 32000) glow += (1 - d2 / 32000) * 0.75;
          }
          const a = 0.07 + glow * 0.5;
          ctx.fillStyle = glow > 0.04 ? `rgba(243,181,98,${Math.min(a, 0.7).toFixed(3)})` : "rgba(255,255,255,0.07)";
          ctx.fillRect(x, y, 1.6, 1.6);
        }
      }
    };

    const loop = (now) => {
      draw(now);
      raf = requestAnimationFrame(loop);
    };

    const play = () => {
      if (running || reduceMotion.matches || !visible || document.hidden) return;
      running = true;
      raf = requestAnimationFrame(loop);
    };

    const pause = () => {
      running = false;
      cancelAnimationFrame(raf);
    };

    resize();
    window.addEventListener("resize", () => {
      clearTimeout(resize.t);
      resize.t = setTimeout(resize, 120);
    }, { passive: true });

    if (finePointer.matches) {
      hero.addEventListener("pointermove", (e) => {
        const rect = hero.getBoundingClientRect();
        pointer.tx = e.clientX - rect.left;
        pointer.ty = e.clientY - rect.top;
        if (pointer.x < -9000) { pointer.x = pointer.tx; pointer.y = pointer.ty; }
      });
      hero.addEventListener("pointerleave", () => { pointer.tx = pointer.ty = pointer.x = pointer.y = -9999; });
    }

    if ("IntersectionObserver" in window) {
      new IntersectionObserver(([entry]) => {
        visible = entry.isIntersecting;
        visible ? play() : pause();
      }).observe(hero);
    }
    document.addEventListener("visibilitychange", () => (document.hidden ? pause() : play()));
    reduceMotion.addEventListener("change", () => (reduceMotion.matches ? (pause(), draw(performance.now())) : play()));
    play();
  }
})();
